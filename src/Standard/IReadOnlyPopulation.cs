using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IReadOnlyPopulation
    {
        ImmutableHashSet<IChromosome> Chromosomes { get; }
    }
}