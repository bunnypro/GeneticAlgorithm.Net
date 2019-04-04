using System.Collections.Generic;

namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IPopulation : IReadOnlyPopulation
    {
        void RegisterOffspring(HashSet<IChromosome> offspring);
    }
}
