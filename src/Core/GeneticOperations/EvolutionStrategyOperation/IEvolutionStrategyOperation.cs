using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.GeneticOperations.EvolutionStrategyOperation
{
    public interface IEvolutionStrategyOperation : IGeneticOperation
    {
        void Prepare(IEnumerable<IChromosome> initialChromosomes);
    }
}