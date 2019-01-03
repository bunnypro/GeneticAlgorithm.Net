using System;
using Bunnypro.GeneticAlgorithm.MultiObjective.Fitness;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Chromosomes
{
    public interface IEvaluableMultiObjectiveFitnessChromosome<in T> where T : Enum
    {
        bool EvaluateFitness(IMultiObjectiveFitnessEvaluator<T> evaluator);
    }
}