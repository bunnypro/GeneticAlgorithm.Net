using System.Collections.Generic;
using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Examples.Simple
{
    public class SimplePopulation : IPopulation
    {
        public ImmutableHashSet<IChromosome> Chromosomes { get; }

        public void Initialize()
        {
        }

        public void StoreOffspring(int evolutionNumber, IEnumerable<IChromosome> offspring)
        {
        }

        public void Reset()
        {
        }
    }
}