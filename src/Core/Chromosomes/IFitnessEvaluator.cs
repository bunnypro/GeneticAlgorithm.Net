using System;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Chromosomes
{
    public interface IFitnessEvaluator
    {
        IComparable Evaluate(IChromosome chromosome);
    }
}