using System;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IEvolutionState
    {
        int EvolutionNumber { get; }
        TimeSpan EvolutionTime { get; }
        bool Evolving { get; }
    }
}
