using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class GeneticAlgorithm
    {
        private readonly GeneticAlgorithmStates _states;
        private readonly IPopulation _population;
        private readonly IGeneticOperation _strategy;
        private readonly SemaphoreSlim _evolutionLock = new SemaphoreSlim(1, 1);

        public GeneticAlgorithm(IPopulation population, IGeneticOperation strategy)
        {
            _states = new GeneticAlgorithmStates();
            _population = population;
            _strategy = strategy;
        }

        public IGeneticAlgorithmStates States => _states;
        public IReadOnlyPopulation Population => _population;

        public async Task<IGeneticAlgorithmCountedStates> EvolveOnce(CancellationToken token = default)
        {
            return await AttemptEvolve(async (GeneticAlgorithmCountedStates states) =>
            {
                var startTime = DateTime.Now;
                var offspring = await _strategy.Operate(_population.Chromosomes, token);
                _population.RegisterOffspring(offspring);
                states.EvolutionTime += DateTime.Now - startTime;
                states.EvolutionCount++;
            }, token);
        }

        private async Task<IGeneticAlgorithmCountedStates> AttemptEvolve(Func<GeneticAlgorithmCountedStates, Task> evolveAction, CancellationToken token)
        {
            try
            {
                await _evolutionLock.WaitAsync(token);
                _states.IsCancelled = false;
                if (!_population.IsInitialized) _population.Initialize();
                var states = new GeneticAlgorithmCountedStates();
                await evolveAction.Invoke(states);
                _states.Merge(states);
                token.ThrowIfCancellationRequested();
                return states;
            }
            catch (OperationCanceledException)
            {
                _states.IsCancelled = true;
                throw;
            }
            finally
            {
                _evolutionLock.Release();
            }
        }

        private class GeneticAlgorithmStates : GeneticAlgorithmCountedStates, IGeneticAlgorithmStates
        {
            public bool IsCancelled { get; set; }
        }

        private class GeneticAlgorithmCountedStates : IGeneticAlgorithmCountedStates
        {
            public int EvolutionCount { get; set; }

            public TimeSpan EvolutionTime { get; set; } = TimeSpan.Zero;

            public void Merge(IGeneticAlgorithmCountedStates states)
            {
                EvolutionCount += states.EvolutionCount;
                EvolutionTime += states.EvolutionTime;
            }
        }
    }
}
