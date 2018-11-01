using System.Collections.Generic;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Strategies
{
    public abstract class EvolutionStrategy<T> : IEvolutionStrategy where T : IChromosome
    {
        public void Prepare(IEnumerable<IChromosome> initialChromosomes)
        {
            Prepare(initialChromosomes.Cast<T>());
        }

        public IEnumerable<IChromosome> GenerateOffspring(IEnumerable<IChromosome> parents)
        {
            return GenerateOffspring(parents.Cast<T>()).Cast<IChromosome>();
        }

        protected abstract void Prepare(IEnumerable<T> initialChromosomes);
        protected abstract IEnumerable<T> GenerateOffspring(IEnumerable<T> parents);
    }
}