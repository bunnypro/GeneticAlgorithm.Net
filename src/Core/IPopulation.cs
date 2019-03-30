using System.Collections.Generic;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IPopulation : IReadOnlyPopulation
    {
        void RegisterOffspring(HashSet<IChromosome> offspring);
    }
}
