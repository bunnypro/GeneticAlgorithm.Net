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
            var result = new GeneticOperationStates();
            await OperateStrategy(population, result, token);
            return result;
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
            while(true) await OperateStrategy(population, states, token);
        }

        public async Task TryEvolve(IPopulation population, GeneticOperationStates result, CancellationToken token)
        {
            try
            {
                await Evolve(population, result, token);
            }
            catch(OperationCanceledException) {}
        }

        public async Task<IGeneticOperationStates> EvolveUntil(
            IPopulation population,
            Func<IGeneticOperationStates, bool> termination)
        {
            var result = new GeneticOperationStates();
            while(!termination.Invoke(result)) await OperateStrategy(population, result, new CancellationToken());
            return result;
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
