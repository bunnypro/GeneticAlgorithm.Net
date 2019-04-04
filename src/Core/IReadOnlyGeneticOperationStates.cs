using System;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IReadOnlyGeneticOperationStates
    {
        int EvolutionCount { get; }
        TimeSpan EvolutionTime { get; }
    }
}
