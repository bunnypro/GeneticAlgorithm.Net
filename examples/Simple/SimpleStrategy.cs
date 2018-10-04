using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Core.EvolutionStrategies;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Examples.Simple
{
    public class SimpleStrategy : EvolutionStrategy
    {
        protected override IEnumerable<IChromosome> GenerateOffspring()
        {
            return new List<IChromosome>();
        }
    }
}