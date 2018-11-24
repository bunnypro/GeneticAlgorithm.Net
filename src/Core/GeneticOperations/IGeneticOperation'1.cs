using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperations
{
    public interface IGeneticOperation<T> : IGeneticOperation where T : IChromosome
    {
        IEnumerable<T> Operate(IEnumerable<T> parents, int count);
    }
}