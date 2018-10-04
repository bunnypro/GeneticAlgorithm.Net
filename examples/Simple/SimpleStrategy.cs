using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Core.EvolutionStrategies;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Examples.Simple
{
    public class SimpleStrategy : EvolutionStrategy
    {
        public override IEnumerable<IChromosome> Execute()
        {
            return new List<IChromosome>();
        }
    }
}