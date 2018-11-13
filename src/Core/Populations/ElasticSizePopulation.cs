using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public class ElasticSizePopulation : Population
    {
        private readonly IChromosomeFactory _chromosomeFactory;

        public ElasticSizePopulation(int minSize, int maxSize, IChromosomeFactory chromosomeFactory)
        {
            MinSize = minSize;
            MaxSize = maxSize;

            _chromosomeFactory = chromosomeFactory;
        }

        public int MinSize { get; }
        public int MaxSize { get; }

        public override int OffspringGenerationSize => new Random().Next(MinSize, MaxSize);

        protected override ImmutableHashSet<IChromosome> CreateInitialChromosomes()
        {
            return _chromosomeFactory.Create(OffspringGenerationSize).ToImmutableHashSet();
        }

        protected override ImmutableHashSet<IChromosome> FilterOffspring(IEnumerable<IChromosome> offspring)
        {
            var uniqueOffspring = new HashSet<IChromosome>(offspring.ToArray());

            if (uniqueOffspring.Count >= MinSize) return uniqueOffspring.Take(new Random().Next(MinSize, Math.Min(MaxSize, uniqueOffspring.Count))).ToImmutableHashSet();

            foreach (var parent in Chromosomes)
                if (uniqueOffspring.Add(parent) && uniqueOffspring.Count == MinSize)
                    break;

            if (uniqueOffspring.Count == MinSize) return uniqueOffspring.ToImmutableHashSet();

            throw new Exception($"Population Size is not Reached. Expected Size: {MinSize}, Filtered Size: {uniqueOffspring.Count}.");
        }
    }
}