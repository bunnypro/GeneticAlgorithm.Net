using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Abstractions;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public class Population : IPopulation
    {
        private ImmutableHashSet<IChromosome> _chromosomes;
        public int GenerationNumber { get; private set; } = -1;

        public virtual ImmutableHashSet<IChromosome> Chromosomes
        {
            get => _chromosomes;
            set
            {
                _chromosomes = value;
                GenerationNumber++;
            }
        }
    }
}