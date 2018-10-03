using System.Collections.Generic;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IEvolutionStrategy
    {
        IEnumerable<IChromosome> Execute(IPopulation population);
    }
}