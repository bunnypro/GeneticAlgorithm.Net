using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.EvolutionStrategies
{
    public abstract class EvolutionStrategy : IEvolutionStrategy
    {
        protected IPopulation Population { get; private set; }

        public virtual void Prepare(IPopulation population)
        {
            Population = population;
            Population.Initialize();
        }

        public virtual void Execute(int evolutionNumber)
        {
            Population.StoreOffspring(evolutionNumber, GenerateOffspring());
        }

        protected abstract IEnumerable<IChromosome> GenerateOffspring();
    }
}