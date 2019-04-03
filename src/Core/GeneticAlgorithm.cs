using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class GeneticAlgorithm
    {
        private readonly GeneticOperationStates _states = new GeneticOperationStates();
        private readonly IGeneticOperation _strategy;

        public GeneticAlgorithm(IGeneticOperation strategy)
        {
            _strategy = strategy;
        }

        public IGeneticOperationStates States => _states;

        public async Task<IGeneticOperationStates> EvolveOnce(
            IPopulation population,
            CancellationToken token = default)
        {
            var states = new GeneticOperationStates();
            await OperateStrategy(population, states, token);
            return states;
        }

        public async Task<bool> TryEvolveOnce(
            IPopulation population,
            GeneticOperationStates states,
            CancellationToken token)
        {
            try
            {
                await OperateStrategy(population, states, token);
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            return true;
        }

        public async Task Evolve(IPopulation population, GeneticOperationStates states, CancellationToken token)
        {
            while (true) await OperateStrategy(population, states, token);
        }

        public async Task TryEvolve(IPopulation population, GeneticOperationStates states, CancellationToken token)
        {
            try
            {
                await Evolve(population, states, token);
            }
            catch (OperationCanceledException) { }
        }

        public async Task<IGeneticOperationStates> EvolveUntil(
            IPopulation population,
            Func<IGeneticOperationStates, bool> termination)
        {
            var states = new GeneticOperationStates();
            await EvolveUntil(population, states, termination, default);
            return states;
        }

        public async Task TryEvolveUntil(
            IPopulation population,
            GeneticOperationStates states,
            Func<IGeneticOperationStates, bool> termination,
            CancellationToken token)
        {
            try
            {
                await EvolveUntil(population, states, termination, token);
            }
            catch (OperationCanceledException) { }
        }

        public async Task EvolveUntil(
            IPopulation population,
            GeneticOperationStates states,
            Func<IGeneticOperationStates, bool> termination,
            CancellationToken token)
        {
            while (!termination.Invoke(states)) await OperateStrategy(population, states, token);
        }

        private async Task OperateStrategy(
            IPopulation population,
            GeneticOperationStates states,
            CancellationToken token)
        {
            var result = new GeneticOperationStates();
            var startTime = DateTime.Now;
            try
            {
                var offspring = await _strategy.Operate(population.Chromosomes, token);
                population.RegisterOffspring(offspring);
                result.EvolutionCount++;
            }
            finally
            {
                result.EvolutionTime += DateTime.Now - startTime;
                _states.Extend(result);
                states.Extend(result);
            }
        }
    }
}
