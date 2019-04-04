using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IReadOnlyPopulation
    {
        ImmutableHashSet<IChromosome> Chromosomes { get; }
    }
}
