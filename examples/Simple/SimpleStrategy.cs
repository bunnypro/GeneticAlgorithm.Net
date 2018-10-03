using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Examples.Simple
{
    public class SimpleStrategy : IEvolutionStrategy
    {
        public IEnumerable<IChromosome> Execute(IPopulation population)
        {
            return new List<IChromosome>();
        }
    }
}