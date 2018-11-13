using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public class FixedSizePopulation : Population
    {
        private readonly IChromosomeFactory _chromosomeFactory;

        public FixedSizePopulation(int size, IChromosomeFactory chromosomeFactory)
        {
            OffspringGenerationSize = size;
            _chromosomeFactory = chromosomeFactory;
        }

        public override int OffspringGenerationSize { get; }

        protected override ImmutableHashSet<IChromosome> CreateInitialChromosomes()
        {
            return _chromosomeFactory.Create(OffspringGenerationSize).ToImmutableHashSet();
        }

        protected override ImmutableHashSet<IChromosome> FilterOffspring(IEnumerable<IChromosome> offspring)
        {
            var uniqueOffspring = new HashSet<IChromosome>(offspring.ToArray());

            if (uniqueOffspring.Count >= OffspringGenerationSize) return uniqueOffspring.Take(OffspringGenerationSize).ToImmutableHashSet();

            foreach (var parent in Chromosomes)
                if (uniqueOffspring.Add(parent) && uniqueOffspring.Count == OffspringGenerationSize)
                    break;

            if (uniqueOffspring.Count == OffspringGenerationSize) return uniqueOffspring.ToImmutableHashSet();

            throw new Exception($"Population Size is not Reached. Expected Size: {OffspringGenerationSize}, Filtered Size: {uniqueOffspring.Count}.");
        }
    }
}