using System;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public struct PopulationCapacity
    {
        public PopulationCapacity(int size) : this(size, size)
        {
        }

        public PopulationCapacity(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }

        public int Minimum { get; }
        public int Maximum { get; }

        public static PopulationCapacity Default => new PopulationCapacity(2, int.MaxValue);
    }
}
