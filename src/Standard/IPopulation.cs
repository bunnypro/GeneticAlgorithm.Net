using System.Collections.Generic;
using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IPopulation
    {
        ImmutableHashSet<IChromosome> Chromosomes { get; }

        void Initialize();
        void StoreOffspring(int generationNumber, IEnumerable<IChromosome> offspring);
        void Reset();
    }
}