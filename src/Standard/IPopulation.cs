using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IPopulation
    {
        ImmutableHashSet<IChromosome> Chromosomes { get; }
    }
}