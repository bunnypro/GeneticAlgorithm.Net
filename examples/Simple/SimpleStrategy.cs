using System.Collections.Generic;
using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Examples.Simple
{
    public class SimpleStrategy : IEvolutionStrategy
    {
        public HashSet<IChromosome> InitialChromosomes { get; private set; }
        
        public void Prepare(IEnumerable<IChromosome> initialChromosomes)
        {
            InitialChromosomes = new HashSet<IChromosome>(initialChromosomes);
        }

        public IEnumerable<IChromosome> GenerateOffspring(IEnumerable<IChromosome> parents)
        {
            return parents.ToImmutableHashSet();
        }
    }
}