using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Chromosomes
{
    public interface IEvaluableFitnessChromosome : IChromosome
    {
        bool EvaluateFitness(IFitnessEvaluator evaluator);
    }
}