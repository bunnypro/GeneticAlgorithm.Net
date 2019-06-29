using System;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Bunnypro.GeneticAlgorithm.Core.Exceptions;
using Bunnypro.GeneticAlgorithm.Primitives;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public sealed class GeneticAlgorithm : IGeneticAlgorithm
    {
        private readonly StatesHolder _statesHolder = new StatesHolder();
        private readonly IGeneticOperation _strategy;

        public GeneticAlgorithm(IGeneticOperation strategy)
        {
            _strategy = strategy;
        }

        public GeneticEvolutionStates EvolutionStates => _statesHolder.States;

        public async Task<GeneticEvolutionStates> EvolveUntil(
            IPopulation population,
            Func<GeneticEvolutionStates, bool> termination,
            CancellationToken token = default)
        {
            var statesHolder = new StatesHolder();
            await Evolve(population, termination, token, statesHolder);
            return statesHolder.States;
        }

        public async Task<(GeneticEvolutionStates, bool)> TryEvolveUntil(
            IPopulation population,
            Func<GeneticEvolutionStates, bool> termination,
            CancellationToken token)
        {
            var statesHolder = new StatesHolder();
            try
            {
                await Evolve(population, termination, token, statesHolder);
            }
            catch (OperationCanceledException)
            {
                return (statesHolder.States, false);
            }

            return (statesHolder.States, true);
        }

        private async Task Evolve(
            IPopulation population,
            Func<GeneticEvolutionStates, bool> termination,
            CancellationToken token,
            StatesHolder statesHolder)
        {
            if (population.IsInitialized)
                throw new UninitializedPopulationException();
            
            try
            {
                while (!termination.Invoke(statesHolder.States))
                {
                    var startTime = DateTime.Now;
                    var offspring = await _strategy.Operate(population.Chromosomes, population.Capacity, token);
                    statesHolder.EvolutionTime += DateTime.Now - startTime;
                    population.Chromosomes = offspring;
                    statesHolder.EvolutionCount++;
                }
            }
            finally
            {
                _statesHolder.Extend(statesHolder.States);
            }
        }

        private class StatesHolder
        {
            public int EvolutionCount { get; set; }
            public TimeSpan EvolutionTime { get; set; } = TimeSpan.Zero;
            public GeneticEvolutionStates States => new GeneticEvolutionStates(EvolutionCount, EvolutionTime);

            public void Extend(GeneticEvolutionStates source)
            {
                EvolutionCount += source.EvolutionCount;
                EvolutionTime += source.EvolutionTime;
            }
        }
    }
}