using System.Collections.Generic;
using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IEvolutionStrategy
    {
        void Prepare(IEnumerable<IChromosome> initialParents);

        IEnumerable<IChromosome> GenerateOffspring(IEnumerable<IChromosome> parents);
    }
}