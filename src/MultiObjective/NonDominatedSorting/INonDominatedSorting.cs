using System.Collections.Generic;
using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.NonDominatedSorting
{
    public interface INonDominatedSorting<T> where T : IChromosome
    {
        IEnumerable<ImmutableArray<T>> Sort(IEnumerable<T> chromosomes);
    }
}