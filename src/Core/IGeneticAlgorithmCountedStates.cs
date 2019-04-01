using System;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IGeneticAlgorithmCountedStates
    {
        int EvolutionCount { get; }
        TimeSpan EvolutionTime { get; }
    }
}
