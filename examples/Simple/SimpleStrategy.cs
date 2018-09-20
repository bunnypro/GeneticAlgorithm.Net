using System.Collections.Generic;
using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Examples.Simple
{
    public class SimpleStrategy : IEvolutionStrategy
    {
        public ImmutableHashSet<IChromosome> Execute(IPopulation population)
        {
            return new List<IChromosome>().ToImmutableHashSet();
        }
    }
}