using System;
using Bunnypro.GeneticAlgorithm.Core.Chromosomes;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Fitness
{
    public interface IMultiObjectiveFitnessEvaluator<out T> : IFitnessEvaluator where T : Enum
    {
        void EvaluateTo(IObjectiveValues<T> value, IChromosome chromosome);
    }
}