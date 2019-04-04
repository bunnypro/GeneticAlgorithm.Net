namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IGeneticOperationStates : IReadOnlyGeneticOperationStates
    {
        void Extend(IReadOnlyGeneticOperationStates source);
    }
}
