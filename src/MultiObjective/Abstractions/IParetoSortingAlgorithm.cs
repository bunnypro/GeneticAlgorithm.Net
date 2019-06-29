using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Abstractions
{
    public interface IParetoSortingAlgorithm<T> where T : Enum
    {
        IEnumerable<ImmutableArray<IChromosome<T>>> Sort(IEnumerable<IChromosome<T>> chromosomes);
    }
}