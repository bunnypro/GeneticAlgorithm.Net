using System;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class GeneticOperationStates : IGeneticOperationStates
    {
        public static GeneticOperationStates From(IReadOnlyGeneticOperationStates source)
        {
            var states = new GeneticOperationStates();
            states.Extend(source);
            return states;
        }

        public int EvolutionCount { get; set; }
        public TimeSpan EvolutionTime { get; set; } = TimeSpan.Zero;

        public void Extend(IReadOnlyGeneticOperationStates source)
        {
            EvolutionCount += source.EvolutionCount;
            EvolutionTime += source.EvolutionTime;
        }
    }
}
