using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IGeneticAlgorithm
    {
        IGeneticEvolutionStates EvolutionStates { get; }

        Task<IGeneticEvolutionStates> EvolveUntil(
            IPopulation population,
            Func<IGeneticEvolutionStates, bool> termination,
            CancellationToken token = default);

        Task<(IGeneticEvolutionStates, bool)> TryEvolveUntil(
            IPopulation population,
            Func<IGeneticEvolutionStates, bool> termination,
            CancellationToken token);
    }
}
