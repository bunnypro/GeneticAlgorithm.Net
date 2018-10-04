using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public class ElasticSizePopulation<T> : Population<T> where T : IChromosome
    {
        public ElasticSizePopulation(int minSize, int maxSize, IChromosomeFactory<T> chromosomeFactory)
        {
            MinSize = minSize;
            MaxSize = maxSize;

            ChromosomeFactory = chromosomeFactory;
        }

        public int MinSize { get; }
        public int MaxSize { get; }

        protected IChromosomeFactory<T> ChromosomeFactory { get; }

        protected override ImmutableHashSet<T> CreatePopulation()
        {
            return ChromosomeFactory.Create(new Random().Next(MinSize, MaxSize)).ToImmutableHashSet();
        }

        protected override ImmutableHashSet<T> FilterOffspring(IEnumerable<T> offspring)
        {
            // ensure distinction

            return offspring.Take(new Random().Next(MinSize, MaxSize)).ToImmutableHashSet();
        }
    }
}