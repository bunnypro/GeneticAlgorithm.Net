using System;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Abstractions;

namespace Bunnypro.GeneticAlgorithm.Extensions
{
    public static class GeneticAlgorithmOperationsExtensions
    {
        public static async Task Evolve(
            this IGeneticAlgorithm genetic,
            IPopulation population,
            CancellationToken token)
        {
            await genetic.EvolveUntil(population, _ => false, token);
        }

        public static async Task<IGeneticEvolutionStates> TryEvolve(
            this IGeneticAlgorithm genetic,
            IPopulation population,
            CancellationToken token)
        {
            var (result, _) = await genetic.TryEvolveUntil(population, _ => false, token);
            return result;
        }
    }
}
