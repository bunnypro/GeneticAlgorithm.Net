using System.Collections.Generic;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IPopulation : IReadOnlyPopulation
    {
        void Initialize();
        void RegisterOffspring(HashSet<IChromosome> offspring);
    }
}
