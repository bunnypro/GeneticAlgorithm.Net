namespace Bunnypro.GeneticAlgorithm.Core.Chromosomes
{
    public interface IEvaluableFitnessChromosome
    {
        bool EvaluateFitness(IFitnessEvaluator evaluator);
    }
}