using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperators
{
    public interface IGeneticOperator<T> : IGeneticOperator where T : IChromosome
    {
        IEnumerable<T> Operate(IEnumerable<T> parents, int count);
    }
}