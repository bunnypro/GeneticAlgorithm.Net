using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public abstract class Population<T> : IPopulation where T : IChromosome
    {
        public int GenerationNumber { get; private set; }

        ImmutableHashSet<IChromosome> IPopulation.Chromosomes => Chromosomes.Cast<IChromosome>().ToImmutableHashSet();
        public ImmutableHashSet<T> Chromosomes { get; private set; }

        public virtual void Initialize()
        {
            GenerationNumber = 0;
            Chromosomes = CreatePopulation();
        }

        protected abstract ImmutableHashSet<T> CreatePopulation();

        public void StoreOffspring(int generationNumber, IEnumerable<IChromosome> offspring)
        {
            GenerationNumber = generationNumber;
            Chromosomes = FilterOffspring(offspring.Cast<T>());
        }

        protected abstract ImmutableHashSet<T> FilterOffspring(IEnumerable<T> offspring);

        public virtual void Reset()
        {
            GenerationNumber = 0;
            Chromosomes = null;
        }
    }
}