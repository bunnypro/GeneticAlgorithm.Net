using System;

namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IGeneticEvolutionStates
    {
        int EvolutionCount { get; }
        TimeSpan EvolutionTime { get; }
    }
}
