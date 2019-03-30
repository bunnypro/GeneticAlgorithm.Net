using System;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IGeneticOperationStates
    {
        int EvolutionCount { get; }
        TimeSpan EvolutionTime { get; }

        void Extend(IGeneticOperationStates states);
    }
}
