using System;
using Bunnypro.GeneticAlgorithm.Abstractions;

namespace Bunnypro.GeneticAlgorithm.TestUtils
{
    public class FakeGeneticOperationState : IGeneticOperationStates
    {
        public int EvolutionCount { get; set; }

        public TimeSpan EvolutionTime { get; set; }

        public void Extend(IReadOnlyGeneticOperationStates source)
        {
            EvolutionCount += source.EvolutionCount;
            EvolutionTime += source.EvolutionTime;
        }
    }
}
