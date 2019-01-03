using System;
using Bunnypro.GeneticAlgorithm.MultiObjective.Fitness;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.MultiObjective
{
    public interface IMultiObjectiveFitnessChromosome<in T> : IChromosome where T : Enum
    {
        new IObjectiveValues<T> Fitness { get; }
    }
}