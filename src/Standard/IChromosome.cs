using System;
using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IChromosome : IEquatable<IChromosome>
    {
        ImmutableList<object> Genes { get; }
        IComparable Fitness { get; }
    }
}