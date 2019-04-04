using System;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Bunnypro.GeneticAlgorithm.Core;

namespace Bunnypro.GeneticAlgorithm.Core.Extensions
{
    public static class GeneticAlgorithmOperationExtensions
    {
        public static Task<bool> TryEvolveOnce(
            this IGeneticAlgorithm genetic,
            IPopulation population,
            IGeneticOperationStates states,
            CancellationToken token)
        {
            return genetic.TryEvolveUntil(population, states, s => s.EvolutionCount >= 1, token);
        }

        public static async Task<bool> TryEvolve(
            this IGeneticAlgorithm genetic,
            IPopulation population,
            IGeneticOperationStates states,
            CancellationToken token)
        {
            try
            {
                await genetic.Evolve(population, states, token);
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            return true;
        }

        public static Task Evolve(
            this IGeneticAlgorithm genetic,
            IPopulation population,
            IGeneticOperationStates states,
            CancellationToken token)
        {
            return genetic.EvolveUntil(population, states, _ => false, token);
        }

        public static async Task<bool> TryEvolveUntil(
            this IGeneticAlgorithm genetic,
            IPopulation population,
            IGeneticOperationStates states,
            Func<IReadOnlyGeneticOperationStates, bool> termination,
            CancellationToken token)
        {
            try
            {
                await genetic.EvolveUntil(population, states, termination, token);
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            return true;
        }
    }
}
