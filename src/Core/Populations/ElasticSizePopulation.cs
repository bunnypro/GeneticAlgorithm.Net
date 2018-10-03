using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Core.Chromosomes;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public class ElasticSizePopulation<T> : Population<T> where T : IChromosome
    {
        public int MinSize { get; }
        public int MaxSize { get; }
        
        protected IChromosomeFactory<T> ChromosomeFactory { get; }

        public ElasticSizePopulation(int minSize, int maxSize, IChromosomeFactory<T> chromosomeFactory)
        {
            MinSize = minSize;
            MaxSize = maxSize;

            ChromosomeFactory = chromosomeFactory;
        }

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