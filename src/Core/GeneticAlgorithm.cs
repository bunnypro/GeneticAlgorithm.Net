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
        public int EvolutionNumber { get; private set; }
        public bool Evolving { get; private set; }

        public IPopulation Population { get; }
        public IEvolutionStrategy EvolutionStrategy { get; }

        public ITerminationCondition TerminationCondition { get; set; }

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

            if (EvolutionNumber == 0)
            {
                Reset();
                Population.Initialize();
            }

            if (TerminationCondition.Fulfilled)
            {
                return;
            }

            if (Evolving)
            {
                throw new EvolutionRunningException();
            }

            Evolving = true;

            using (_evolutionCts = new CancellationTokenSource())
            {
                TerminationCondition.Start();

                await Task.Factory.StartNew(() =>
                {
                    do
                    {
                        Population.StoreOffspring(EvolutionNumber++, EvolutionStrategy.Execute(Population));
                    } while (!(_evolutionCts.Token.IsCancellationRequested || TerminationCondition.Fulfilled));

                    if (_evolutionCts.Token.IsCancellationRequested)
                    {
                        TerminationCondition.Pause();
                    }
                }, _evolutionCts.Token);
            }

            Evolving = false;
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

            EvolutionNumber = 0;
            _evolutionCts = null;

            TerminationCondition.Reset();
            Population.Reset();
        }
    }
}