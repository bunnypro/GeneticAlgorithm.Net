using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperators
{
    public interface IGeneticOperator
    {
        IEnumerable<IChromosome> Operate(IEnumerable<IChromosome> parents, int count);
    }
}