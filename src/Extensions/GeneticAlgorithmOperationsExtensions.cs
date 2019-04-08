using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Bunnypro.GeneticAlgorithm.Primitives;

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

        public static async Task<GeneticEvolutionStates> TryEvolve(
            this IGeneticAlgorithm genetic,
            IPopulation population,
            CancellationToken token)
        {
            var (result, _) = await genetic.TryEvolveUntil(population, _ => false, token);
            return result;
        }
    }
}