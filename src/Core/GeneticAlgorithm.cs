using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Core.Termination;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class GeneticAlgorithm : IGeneticAlgorithm
    {
        public int GenerationNumber { get; private set; }

        public bool Evolving
        {
            get
            {
                var evolvingObjectCanBeAcquired = Monitor.TryEnter(_evolving);
                
                if (evolvingObjectCanBeAcquired)
                {
                    Monitor.Exit(_evolving);
                }

                return !evolvingObjectCanBeAcquired;
            }
        }
        
        public IPopulation Population { get; }
        public IEvolutionStrategy EvolutionStrategy { get; }
        
        public ITermination Termination { get; set; }

        private readonly object _evolving = new object();
        
        private Task _evolution;
        private CancellationTokenSource _evolutionTokenSource;

        public GeneticAlgorithm(IPopulation population, IEvolutionStrategy evolutionStrategy)
        {
            Population = population;
            EvolutionStrategy = evolutionStrategy;
        }

        public async Task Evolve()
        {
            await EvolveUntil(Termination ?? new FunctionTermination(() => false));
        }

        public async Task EvolveUntil(Func<bool> fulfilled)
        {
            await EvolveUntil(new FunctionTermination(fulfilled));
        }

        public async Task EvolveUntil(ITermination termination)
        {
            Termination = termination;
            
            if (Termination.Fulfilled())
            {
                _evolutionTokenSource.Dispose();
                return;
            }
            
            if (GenerationNumber == 0)
            {
                await Reset();
                Population.Initialize();
            }
            
            Termination.Start();

            using (_evolutionTokenSource = new CancellationTokenSource())
            {
                _evolution = Task.Factory.StartNew(() =>
                {
                    lock (_evolving)
                    {
                        do
                        {
                            Population.StoreOffspring(GenerationNumber++, EvolutionStrategy.Execute(Population));
                        } while (!(Termination.Fulfilled() || _evolutionTokenSource.Token.IsCancellationRequested));

                        if (_evolutionTokenSource.Token.IsCancellationRequested)
                        {
                            Termination.Pause();
                        }
                    }
                }, _evolutionTokenSource.Token);

                await _evolution;
            }
        }

        public async Task Stop()
        {
            _evolutionTokenSource?.Cancel();
            await _evolution;
        }

        public async Task Reset()
        {
            if (Evolving) await Task.Run(() => { Monitor.Wait(_evolving); });
            
            lock (_evolving)
            {
                GenerationNumber = 0;
                _evolution = null;
                _evolutionTokenSource = null;

                Termination.Reset();
                Population.Reset();
            }
        }
    }
}