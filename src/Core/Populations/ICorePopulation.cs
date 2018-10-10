using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public interface ICorePopulation : IPopulation
    {
        void Initialize();
        void StoreOffspring(IEnumerable<IChromosome> offspring);
    }
}