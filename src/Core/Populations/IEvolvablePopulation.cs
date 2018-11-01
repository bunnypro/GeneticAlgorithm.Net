using System.Collections.Generic;
using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public interface IEvolvablePopulation : IPopulation
    {
        int GenerationNumber { get; }
        ImmutableHashSet<IChromosome> InitialChromosomes { get; }

        void Initialize();
        void StoreOffspring(IEnumerable<IChromosome> offspring);
    }
}