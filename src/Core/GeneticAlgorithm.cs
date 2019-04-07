using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Abstractions;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public sealed class GeneticAlgorithm : IGeneticAlgorithm
    {
        private readonly IGeneticOperation _strategy;
        private GeneticEvolutionStates _states = new GeneticEvolutionStates();

        public GeneticAlgorithm(IGeneticOperation strategy)
        {
            _strategy = strategy;
        }

        public IGeneticEvolutionStates EvolutionStates => _states.Clone();

        public async Task<IGeneticEvolutionStates> EvolveUntil(
            IPopulation population,
            Func<IGeneticEvolutionStates, bool> termination,
            CancellationToken token = default)
        {
            GeneticEvolutionStates states = new GeneticEvolutionStates();
            await Evolve(population, termination, token, states);
            return states;
        }

        public async Task<(IGeneticEvolutionStates, bool)> TryEvolveUntil(
            IPopulation population,
            Func<IGeneticEvolutionStates, bool> termination,
            CancellationToken token)
        {
            GeneticEvolutionStates states = new GeneticEvolutionStates();
            try
            {
                await Evolve(population, termination, token, states);
            }
            catch (OperationCanceledException)
            {
                return (states, false);
            }
            return (states, true);
        }

        private async Task Evolve(
            IPopulation population,
            Func<IGeneticEvolutionStates, bool> termination,
            CancellationToken token,
            GeneticEvolutionStates states)
        {
            try
            {
                while (!termination.Invoke(states))
                {
                    var startTime = DateTime.Now;
                    population.Chromosomes = await _strategy.Operate(population.Chromosomes, token);
                    states.EvolutionTime += DateTime.Now - startTime;
                    states.EvolutionCount++;
                }
            }
            finally
            {
                _states.Extend(states);
            }
        }

        private class GeneticEvolutionStates : IGeneticEvolutionStates
        {
            public int EvolutionCount { get; set; }
            public TimeSpan EvolutionTime { get; set; }

            public void Extend(GeneticEvolutionStates states)
            {
                EvolutionCount += states.EvolutionCount;
                EvolutionTime += states.EvolutionTime;
            }

            public GeneticEvolutionStates Clone()
            {
                return (GeneticEvolutionStates)MemberwiseClone();
            }
        }
    }
}
