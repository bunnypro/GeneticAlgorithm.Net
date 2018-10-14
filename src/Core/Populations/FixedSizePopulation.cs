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
            Size = size;
            _chromosomeFactory = chromosomeFactory;
        }

        public int Size { get; }

        protected override ImmutableHashSet<IChromosome> CreateInitialChromosomes()
        {
            return _chromosomeFactory.Create(Size).ToImmutableHashSet();
        }

        protected override ImmutableHashSet<IChromosome> FilterOffspring(IEnumerable<IChromosome> offspring)
        {
            var uniqueOffspring = new HashSet<IChromosome>(offspring.ToArray());

            if (uniqueOffspring.Count >= Size) return uniqueOffspring.Take(Size).ToImmutableHashSet();

            foreach (var parent in Chromosomes)
                if (uniqueOffspring.Add(parent) && uniqueOffspring.Count == Size)
                    break;

            if (uniqueOffspring.Count == Size) return uniqueOffspring.ToImmutableHashSet();

            throw new Exception($"Population Size is not Reached. Expected Size: {Size}, Filtered Size: {uniqueOffspring.Count}.");
        }
    }
}
