using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.EvolutionStrategies
{
    public abstract class EvolutionStrategy : IEvolutionStrategy
    {
        public virtual void Prepare(IPopulation population)
        {
            population.Initialize();
        }

        public abstract IEnumerable<IChromosome> Execute(IPopulation population);
    }
}