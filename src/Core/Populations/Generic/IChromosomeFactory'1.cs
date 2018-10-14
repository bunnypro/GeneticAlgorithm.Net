using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations.Generic
{
    public interface IChromosomeFactory<T> : IChromosomeFactory where T : IChromosome
    {
        new T Create();
        new HashSet<T> Create(int count);
    }
}
