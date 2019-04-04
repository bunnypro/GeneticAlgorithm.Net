using System;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Abstractions;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class GeneticAlgorithm : IGeneticAlgorithm
    {
        private readonly IGeneticOperationStates _states = new GeneticOperationStates();
        private readonly IGeneticOperation _strategy;

        public GeneticAlgorithm(IGeneticOperation strategy)
        {
            _strategy = strategy;
        }

        public IReadOnlyGeneticOperationStates States => _states;

        public async Task<IReadOnlyGeneticOperationStates> EvolveOnce(
            IPopulation population,
            CancellationToken token = default)
        {
            var states = new GeneticOperationStates();
            await EvolveUntil(population, states, s => s.EvolutionCount >= 1, token);
            return states;
        }

        public async Task<IReadOnlyGeneticOperationStates> EvolveUntil(
            IPopulation population,
            Func<IReadOnlyGeneticOperationStates, bool> termination)
        {
            var states = new GeneticOperationStates();
            await EvolveUntil(population, states, termination, default);
            return states;
        }

        public async Task EvolveUntil(
            IPopulation population,
            IGeneticOperationStates states,
            Func<IReadOnlyGeneticOperationStates, bool> termination,
            CancellationToken token)
        {
            while (!termination.Invoke(states)) await OperateStrategy(population, states, token);
        }

        private async Task OperateStrategy(
            IPopulation population,
            IGeneticOperationStates states,
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
                lock (_states)
                {
                    _states.Extend(result);
                }
                states.Extend(result);
            }
        }
    }
}
