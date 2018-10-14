using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public class ElasticSizePopulation<T> : Population<T> where T : IChromosome
    {
        private readonly IChromosomeFactory<T> _chromosomeFactory;
        
        public ElasticSizePopulation(int minSize, int maxSize, IChromosomeFactory<T> chromosomeFactory)
        {
            MinSize = minSize;
            MaxSize = maxSize;

            _chromosomeFactory = chromosomeFactory;
        }

        public int MinSize { get; }
        public int MaxSize { get; }

        protected override ImmutableHashSet<T> CreateInitialChromosomes()
        {
            return _chromosomeFactory.Create(new Random().Next(MinSize, MaxSize)).ToImmutableHashSet();
        }

        protected override ImmutableHashSet<T> FilterOffspring(IEnumerable<T> offspring)
        {
            var uniqueOffspring = new HashSet<T>(offspring.ToArray());

            if (uniqueOffspring.Count >= MinSize) return uniqueOffspring.Take(new Random().Next(MinSize, Math.Min(MaxSize, uniqueOffspring.Count))).ToImmutableHashSet();

            foreach (var parent in Chromosomes)
                if (uniqueOffspring.Add(parent) && uniqueOffspring.Count == MinSize)
                    break;

            if (uniqueOffspring.Count == MinSize) return uniqueOffspring.ToImmutableHashSet();

            throw new Exception($"Population Size is not Reached. Expected Size: {MinSize}, Filtered Size: {uniqueOffspring.Count}.");
        }
    }
}