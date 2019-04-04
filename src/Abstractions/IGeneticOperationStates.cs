namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IGeneticOperationStates : IReadOnlyGeneticOperationStates
    {
        void Extend(IReadOnlyGeneticOperationStates source);
    }
}
