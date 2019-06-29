using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Primitives;

namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IPopulation
    {
        PopulationCapacity Capacity { get; }
        ImmutableHashSet<IChromosome> Chromosomes { get; set; }
    }
}