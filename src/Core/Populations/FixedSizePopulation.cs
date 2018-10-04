using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public class FixedSizePopulation<T> : Population<T> where T : IChromosome
    {
        public FixedSizePopulation(int size, IChromosomeFactory<T> chromosomeFactory)
        {
            Size = size;
            ChromosomeFactory = chromosomeFactory;
        }

        public int Size { get; }

        protected IChromosomeFactory<T> ChromosomeFactory { get; }

        protected override ImmutableHashSet<T> CreatePopulation()
        {
            return ChromosomeFactory.Create(Size).ToImmutableHashSet();
        }

        protected override ImmutableHashSet<T> FilterOffspring(IEnumerable<T> offspring)
        {
            // ensure distinction

            return offspring.Take(Size).ToImmutableHashSet();
        }
    }
}