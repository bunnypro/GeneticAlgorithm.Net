using System.Collections.Generic;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Strategies
{
    public interface IEvolutionStrategy
    {
        void Prepare(IEnumerable<IChromosome> initialChromosomes);
        IEnumerable<IChromosome> GenerateOffspring(IEnumerable<IChromosome> parents);
    }
}