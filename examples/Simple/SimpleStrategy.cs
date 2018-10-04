using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Core.EvolutionStrategies;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Examples.Simple
{
    public class SimpleStrategy : EvolutionStrategy
    {
        public override IEnumerable<IChromosome> Execute(IPopulation population)
        {
            return new List<IChromosome>();
        }
    }
}