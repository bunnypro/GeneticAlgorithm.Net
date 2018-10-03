using System;
using System.Collections.Generic;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IChromosome : IEquatable<IChromosome>
    {
        IReadOnlyCollection<object> Genes { get; }
        IComparable Fitness { get; }
    }
}