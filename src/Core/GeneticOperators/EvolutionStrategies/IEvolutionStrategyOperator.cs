using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperators.EvolutionStrategies
{
    public interface IEvolutionStrategyOperator : IGeneticOperator
    {
        void Prepare(IEnumerable<IChromosome> initialChromosomes);
    }
}