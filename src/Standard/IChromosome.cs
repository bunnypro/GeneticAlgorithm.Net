using System;
using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IChromosome : IEquatable<IChromosome>
    {
        ImmutableArray<object> Genes { get; }
        IComparable Fitness { get; }
    }
}