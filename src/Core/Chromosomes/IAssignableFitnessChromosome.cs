using System;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Chromosomes
{
    public interface IAssignableFitnessChromosome : IChromosome
    {
        new IComparable Fitness { get; set; }
    }
}