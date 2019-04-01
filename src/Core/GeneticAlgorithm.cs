using System;
using System.Threading;
using System.Threading.Tasks;

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
            return await AttemptEvolve(states => OperateStrategy(states, token), token);
        }

        public async Task<IGeneticAlgorithmCountedStates> Evolve(CancellationToken token)
        {
            return await AttemptEvolve(async states =>
            {
                while (true) await OperateStrategy(states, token);
            }, token);
        }

        public async Task<IGeneticAlgorithmCountedStates> EvolveUntil(Func<IGeneticAlgorithmCountedStates, bool> termination)
        {
            return await AttemptEvolve(async states =>
            {
                while (!termination.Invoke(states)) await OperateStrategy(states);
            });
        }

        private async Task OperateStrategy(GeneticAlgorithmCountedStates states, CancellationToken token = default)
        {
            var startTime = DateTime.Now;
            try
            {
                var offspring = await _strategy.Operate(_population.Chromosomes, token);
                _population.RegisterOffspring(offspring);
                states.EvolutionCount++;
            }
            finally
            {
                states.EvolutionTime += DateTime.Now - startTime;
            }
        }

        private async Task<IGeneticAlgorithmCountedStates> AttemptEvolve(
            Func<GeneticAlgorithmCountedStates, Task> evolveAction,
            CancellationToken token = default)
        {
            var states = new GeneticAlgorithmCountedStates();
            var lockAcquired = false;
            try
            {
                await _evolutionLock.WaitAsync(token);
                lockAcquired = true;
                _states.IsCanceled = false;
                if (!_population.IsInitialized) _population.Initialize();
                await evolveAction.Invoke(states);
            }
            catch (OperationCanceledException)
            {
                _states.IsCanceled = true;
                throw;
            }
            finally
            {
                _states.Merge(states);
                if (lockAcquired) _evolutionLock.Release();
            }
            return states;
        }

        private class GeneticAlgorithmStates : GeneticAlgorithmCountedStates, IGeneticAlgorithmStates
        {
            public bool IsCanceled { get; set; }
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
