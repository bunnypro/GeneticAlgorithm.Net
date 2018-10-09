using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public abstract class Population<T> : IPopulation where T : IChromosome
    {
        public int GenerationNumber { get; private set; }
        public ImmutableHashSet<T> Chromosomes { get; private set; }

        ImmutableHashSet<IChromosome> IReadOnlyPopulation.Chromosomes => Chromosomes?.Cast<IChromosome>().ToImmutableHashSet();

        public virtual void Initialize()
        {
            GenerationNumber = 0;
            Chromosomes = CreateInitialChromosomes();
        }

        public void StoreOffspring(IEnumerable<IChromosome> offspring)
        {
            GenerationNumber++;
            Chromosomes = FilterOffspring(offspring.Cast<T>());
        }

        protected abstract ImmutableHashSet<T> CreateInitialChromosomes();

        protected abstract ImmutableHashSet<T> FilterOffspring(IEnumerable<T> offspring);
    }
}