using System.Collections.Generic;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperators
{
    public abstract class GeneticOperator<T> : IGeneticOperator where T : IChromosome
    {
        public IEnumerable<IChromosome> Operate(IEnumerable<IChromosome> parents, int count)
        {
            return Operate(parents.Cast<T>(), count).Cast<IChromosome>();
        }
        
        public abstract IEnumerable<T> Operate(IEnumerable<T> parents, int count);
    }
}