using System.Collections.Generic;
using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IPopulation : IReadOnlyPopulation
    {
        void Initialize();
        void StoreOffspring(IEnumerable<IChromosome> offspring);
    }
}