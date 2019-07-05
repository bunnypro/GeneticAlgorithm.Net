using System;
using System.Collections.Generic;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Abstractions
{
    public interface IFitnessEvaluator<T> where T : Enum
    {
        double Evaluate(IChromosome<T> chromosome);
        void EvaluateAll(IEnumerable<IChromosome<T>> chromosomes);
    }
}