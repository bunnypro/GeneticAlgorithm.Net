using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IReadOnlyPopulation
    {
        int GenerationNumber { get; }
        bool IsInitialized { get; }
        ImmutableHashSet<IChromosome> Chromosomes { get; }
    }
}