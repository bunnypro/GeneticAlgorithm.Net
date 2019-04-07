using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IPopulation
    {
        ImmutableHashSet<IChromosome> Chromosomes { get; set; }
    }
}
