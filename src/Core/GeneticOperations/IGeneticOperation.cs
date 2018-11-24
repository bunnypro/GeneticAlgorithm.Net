using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperations
{
    public interface IGeneticOperation
    {
        IEnumerable<IChromosome> Operate(IEnumerable<IChromosome> parents, int count);
    }
}