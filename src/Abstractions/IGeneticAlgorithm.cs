using System;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Primitives;

namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IGeneticAlgorithm
    {
        GeneticEvolutionStates EvolutionStates { get; }

        Task<GeneticEvolutionStates> EvolveUntil(
            IPopulation population,
            Func<GeneticEvolutionStates, bool> termination,
            CancellationToken token = default);

        Task<(GeneticEvolutionStates, bool)> TryEvolveUntil(
            IPopulation population,
            Func<GeneticEvolutionStates, bool> termination,
            CancellationToken token);
    }
}