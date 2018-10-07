using System.Collections.Generic;
using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Examples.Simple
{
    public class SimpleStrategy : IEvolutionStrategy
    {
        public void Prepare(IEnumerable<IChromosome> initialChromosomes)
        {
        }

        public IEnumerable<IChromosome> GenerateOffspring(IEnumerable<IChromosome> parents)
        {
            return parents.ToImmutableHashSet();
        }
    }
}