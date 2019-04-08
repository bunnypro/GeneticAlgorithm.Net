using System;

namespace Bunnypro.GeneticAlgorithm.Primitives
{
    public struct GeneticEvolutionStates
    {
        public GeneticEvolutionStates(int count, TimeSpan time)
        {
            EvolutionCount = count;
            EvolutionTime = time;
        }

        public int EvolutionCount { get; }
        public TimeSpan EvolutionTime { get; }
    }
}