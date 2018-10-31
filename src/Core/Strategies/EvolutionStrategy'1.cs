using System.Collections.Generic;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Strategies
{
    public abstract class EvolutionStrategy<T> : IEvolutionStrategy, IEvolutionStrategy<T> where T : IChromosome
    {
        void IEvolutionStrategy.Prepare(IEnumerable<IChromosome> initialChromosomes)
        {
            Prepare(initialChromosomes.Cast<T>());
        }

        IEnumerable<IChromosome> IEvolutionStrategy.GenerateOffspring(IEnumerable<IChromosome> parents)
        {
            return GenerateOffspring(parents.Cast<T>()).Cast<IChromosome>();
        }

        public abstract void Prepare(IEnumerable<T> initialChromosomes);
        public abstract IEnumerable<T> GenerateOffspring(IEnumerable<T> parents);
    }
}
