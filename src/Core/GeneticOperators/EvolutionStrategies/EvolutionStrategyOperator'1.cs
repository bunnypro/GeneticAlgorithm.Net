using System.Collections.Generic;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperators.EvolutionStrategies
{
    public abstract class EvolutionStrategyOperator<T> : GeneticOperator<T>, IEvolutionStrategyOperator where T : IChromosome
    {
        public void Prepare(IEnumerable<IChromosome> initialChromosomes)
        {
            Prepare(initialChromosomes.Cast<T>());
        }
        
        protected abstract void Prepare(IEnumerable<T> initialChromosomes);
    }
}