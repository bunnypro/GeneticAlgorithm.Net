using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations.Generic
{
    public interface IChromosomeFactory<T> where T : IChromosome
    {
        T Create();
        HashSet<T> Create(int count);
    }
}
