using System;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class GeneticOperationStates : IGeneticOperationStates
    {
        public int EvolutionCount { get; set; }
        public TimeSpan EvolutionTime { get; set; } = TimeSpan.Zero;

        public void Extend(IGeneticOperationStates states)
        {
            EvolutionCount += states.EvolutionCount;
            EvolutionTime += states.EvolutionTime;
        }
    }
}
