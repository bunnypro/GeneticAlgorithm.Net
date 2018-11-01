using System.Collections.Generic;
using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public abstract class Population : IEvolvablePopulation
    {
        public int GenerationNumber { get; private set; }
        public ImmutableHashSet<IChromosome> InitialChromosomes { get; private set; }
        public ImmutableHashSet<IChromosome> Chromosomes { get; private set; }

        public void Initialize()
        {
            GenerationNumber = 0;
            Chromosomes = InitialChromosomes = CreateInitialChromosomes();
        }

        public void StoreOffspring(IEnumerable<IChromosome> offspring)
        {
            GenerationNumber++;
            Chromosomes = FilterOffspring(offspring);
        }

        protected abstract ImmutableHashSet<IChromosome> CreateInitialChromosomes();

        protected abstract ImmutableHashSet<IChromosome> FilterOffspring(IEnumerable<IChromosome> offspring);
    }
}