using System.Collections.Generic;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IEvolutionStrategy
    {
        void Prepare(IPopulation population);
        IEnumerable<IChromosome> Execute(IPopulation population);
    }
}