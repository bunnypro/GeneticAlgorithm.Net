using System.Collections.Generic;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperators.EvolutionStrategies
{
    public abstract class EvolutionStrategyOperator<T> : IEvolutionStrategyOperator, IGeneticOperator<T> where T : IChromosome
    {
        public void Prepare(IEnumerable<IChromosome> initialChromosomes)
        {
            Prepare(initialChromosomes.Cast<T>());
        }

        public IEnumerable<IChromosome> Operate(IEnumerable<IChromosome> parents, int count)
        {
            return Operate(parents.Cast<T>(), count).Cast<IChromosome>();
        }
        
        public abstract void Prepare(IEnumerable<T> initialChromosomes);
        public abstract IEnumerable<T> Operate(IEnumerable<T> parents, int count);
    }
}