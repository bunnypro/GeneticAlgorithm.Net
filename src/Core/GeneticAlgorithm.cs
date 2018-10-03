using System;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Core.Exceptions;
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

        public ITerminationCondition TerminationCondition { get; set; }

        private readonly object _evolving = new object();

        private CancellationTokenSource _evolutionCts;

        public GeneticAlgorithm(IPopulation population, IEvolutionStrategy evolutionStrategy)
        {
            Population = population;
            EvolutionStrategy = evolutionStrategy;
        }

        public async Task Evolve()
        {
            await EvolveUntil(TerminationCondition ?? new FunctionTerminationCondition(() => false));
        }

        public async Task EvolveUntil(Func<bool> fulfilled)
        {
            await EvolveUntil(new FunctionTerminationCondition(fulfilled));
        }

        public async Task EvolveUntil(ITerminationCondition terminationCondition)
        {
            TerminationCondition = terminationCondition;

            if (GenerationNumber == 0)
            {
                Reset();
                Population.Initialize();
            }

            if (TerminationCondition.Fulfilled)
            {
                return;
            }

            TerminationCondition.Start();

            using (_evolutionCts = new CancellationTokenSource())
            {
                await Task.Factory.StartNew(() =>
                {
                    lock (_evolving)
                    {
                        do
                        {
                            Population.StoreOffspring(GenerationNumber++, EvolutionStrategy.Execute(Population));
                        } while (!(_evolutionCts.Token.IsCancellationRequested || TerminationCondition.Fulfilled));

                        if (_evolutionCts.Token.IsCancellationRequested)
                        {
                            TerminationCondition.Pause();
                        }
                    }
                }, _evolutionCts.Token);
            }
        }

        public void Stop()
        {
            _evolutionCts?.Cancel();
        }

        public bool TryReset()
        {
            try
            {
                Reset();
            }
            catch (EvolutionRunningException)
            {
                return false;
            }

            return true;
        }

        public void Reset()
        {
            if (Evolving)
            {
                throw new EvolutionRunningException();
            }

            lock (_evolving)
            {
                GenerationNumber = 0;
                _evolutionCts = null;

                TerminationCondition.Reset();
                Population.Reset();
            }
        }
    }
}
