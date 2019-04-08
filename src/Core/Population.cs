using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Bunnypro.GeneticAlgorithm.Core.Exceptions;
using Bunnypro.GeneticAlgorithm.Primitives;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class Population : IPopulation
    {
        private ImmutableHashSet<IChromosome> _chromosomes;
        public int GenerationNumber { get; private set; } = -1;
        public PopulationCapacity Capacity { get; set; } = PopulationCapacity.Default;

        public virtual ImmutableHashSet<IChromosome> Chromosomes
        {
            get => _chromosomes;
            set
            {
                if (value.Count < Capacity.Minimum || value.Count > Capacity.Maximum)
                    throw new SizeOutOfCapacityException();
                _chromosomes = value;
                GenerationNumber++;
            }
        }
    }
}