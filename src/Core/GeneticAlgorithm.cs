using System;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Core.EvolutionStrategies;
using Bunnypro.GeneticAlgorithm.Core.Exceptions;
using Bunnypro.GeneticAlgorithm.Core.Terminations;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class GeneticAlgorithm : IGeneticAlgorithm
    {
        private readonly object _evolutionPreparation = new object();
        private CancellationTokenSource _evolutionCts;

        public GeneticAlgorithm(IPopulation population, EvolutionStrategy evolutionStrategy)
        {
            Population = population;
            EvolutionStrategy = evolutionStrategy;
        }

        public IPopulation Population { get; }
        public EvolutionStrategy EvolutionStrategy { get; }

        public ITerminationCondition TerminationCondition { get; set; }
        public int EvolutionNumber { get; private set; }
        public bool Evolving { get; private set; }

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
            lock (_evolutionPreparation)
            {
                if (Evolving) throw new EvolutionRunningException();

                TerminationCondition = terminationCondition;

                if (EvolutionNumber == 0)
                {
                    Prepare();
                    EvolutionStrategy.Prepare(Population);
                }

                if (TerminationCondition.Fulfilled) return;

                Evolving = true;
            }

            using (_evolutionCts = new CancellationTokenSource())
            {
                TerminationCondition.Start();

                await Task.Factory.StartNew(() =>
                {
                    do
                    {
                        Population.StoreOffspring(EvolutionNumber++, EvolutionStrategy.Execute());
                    } while (!(_evolutionCts.Token.IsCancellationRequested || TerminationCondition.Fulfilled));

                    if (_evolutionCts.Token.IsCancellationRequested) TerminationCondition.Pause();
                }, _evolutionCts.Token);
            }

            Evolving = false;
        }

        public void Stop()
        {
            _evolutionCts?.Cancel();
        }

        public void Reset()
        {
            if (Evolving) throw new EvolutionRunningException();

            Prepare();
        }

        private void Prepare()
        {
            EvolutionNumber = 0;
            _evolutionCts = null;

            TerminationCondition.Reset();
            Population.Reset();
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
    }
}