namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IGeneticAlgorithmStates: IGeneticAlgorithmCountedStates
    {
        bool IsCanceled { get; }
    }
}
