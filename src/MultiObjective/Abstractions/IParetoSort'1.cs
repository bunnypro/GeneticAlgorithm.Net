using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Abstractions
{
    public interface IParetoSort<T> where T : Enum
    {
        IEnumerable<ImmutableArray<IChromosome<T>>> Sort(IEnumerable<IChromosome<T>> chromosomes);
    }
}