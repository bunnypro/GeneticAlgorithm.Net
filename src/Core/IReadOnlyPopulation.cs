using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IReadOnlyPopulation
    {
        ImmutableHashSet<IChromosome> Chromosomes { get; }
    }
}
