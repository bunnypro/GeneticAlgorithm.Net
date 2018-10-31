using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Strategies
{
    public interface IEvolutionStrategy<T> where T : IChromosome
    {
        void Prepare(IEnumerable<T> initialChromosomes);
        IEnumerable<T> GenerateOffspring(IEnumerable<T> parents);
    }
}