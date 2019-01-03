using System;
using Bunnypro.GeneticAlgorithm.Core.Chromosomes;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Fitness
{
    public abstract class FitnessEvaluator<T> : IFitnessEvaluator where T : IChromosome
    {
        public IComparable Evaluate(IChromosome chromosome)
        {
            return Evaluate((T) chromosome);
        }
        
        protected abstract IComparable Evaluate(T chromosome);
    }
}