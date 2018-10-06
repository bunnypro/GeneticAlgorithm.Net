using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Core.Populations;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Examples.Simple
{
    public class SimplePopulation : Population<SimpleChromosome>
    {
        protected override ImmutableHashSet<SimpleChromosome> CreatePopulation()
        {
            return new List<SimpleChromosome> {new SimpleChromosome(new List<object> {new Random().Next(100)})}.ToImmutableHashSet();
        }

        protected override ImmutableHashSet<SimpleChromosome> FilterOffspring(IEnumerable<SimpleChromosome> offspring)
        {
            return offspring.ToImmutableHashSet();
        }
    }
}