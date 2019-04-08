namespace Bunnypro.GeneticAlgorithm.Primitives
{
    public struct PopulationCapacity
    {
        public static PopulationCapacity Default => new PopulationCapacity(2, int.MaxValue);

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
    }
}