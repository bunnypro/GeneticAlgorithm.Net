namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IGeneticAlgorithmStates: IGeneticAlgorithmCountedStates
    {
        bool IsCancelled { get; }
    }
}
