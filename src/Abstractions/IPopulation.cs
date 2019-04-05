using System.Collections.Generic;
using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IPopulation : IReadOnlyPopulation
    {
        new ImmutableHashSet<IChromosome> Chromosomes { get; set; }
    }
}
