using System.Collections.Generic;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperations
{
    public abstract class GeneticOperation<T> : IGeneticOperation where T : IChromosome
    {
        public IEnumerable<IChromosome> Operate(IEnumerable<IChromosome> parents, int count)
        {
            return Operate(parents.Cast<T>(), count).Cast<IChromosome>();
        }
        
        public abstract IEnumerable<T> Operate(IEnumerable<T> parents, int count);
    }
}