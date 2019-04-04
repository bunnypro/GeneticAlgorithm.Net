using System;

namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IReadOnlyGeneticOperationStates
    {
        int EvolutionCount { get; }
        TimeSpan EvolutionTime { get; }
    }
}
