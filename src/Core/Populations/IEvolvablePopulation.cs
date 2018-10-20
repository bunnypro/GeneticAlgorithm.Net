using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public interface IEvolvablePopulation : IPopulation
    {
        int GenerationNumber { get; }

        void Initialize();
        void StoreOffspring(IEnumerable<IChromosome> offspring);
    }
}
