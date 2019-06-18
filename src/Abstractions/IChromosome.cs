using System;

namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IChromosome
    {
        IComparable Fitness { get; }
    }
}