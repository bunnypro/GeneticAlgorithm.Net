using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public interface IChromosomeFactory
    {
        IChromosome Create();
        HashSet<IChromosome> Create(int count);
    }
}