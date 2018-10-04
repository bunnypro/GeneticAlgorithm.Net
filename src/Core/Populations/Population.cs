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

        ImmutableHashSet<IChromosome> IPopulation.Chromosomes => Chromosomes.Cast<IChromosome>().ToImmutableHashSet();

        public virtual void Initialize()
        {
            GenerationNumber = 0;
            Chromosomes = CreatePopulation();
        }

        public void StoreOffspring(int generationNumber, IEnumerable<IChromosome> offspring)
        {
            GenerationNumber = generationNumber;
            Chromosomes = FilterOffspring(offspring.Cast<T>());
        }

        public virtual void Reset()
        {
            GenerationNumber = 0;
            Chromosomes = null;
        }

        protected abstract ImmutableHashSet<T> CreatePopulation();

        protected abstract ImmutableHashSet<T> FilterOffspring(IEnumerable<T> offspring);
    }
}