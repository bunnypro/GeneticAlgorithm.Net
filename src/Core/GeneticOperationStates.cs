using System;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class GeneticOperationStates : IGeneticOperationStates
    {
        public static GeneticOperationStates From(IGeneticOperationStates source)
        {
            var states = new GeneticOperationStates();
            states.Extend(source);
            return states;
        }

        public int EvolutionCount { get; set; }
        public TimeSpan EvolutionTime { get; set; } = TimeSpan.Zero;

        public void Extend(IGeneticOperationStates states)
        {
            EvolutionCount += states.EvolutionCount;
            EvolutionTime += states.EvolutionTime;
        }
    }
}
