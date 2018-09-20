using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IEvolutionStrategy
    {
        ImmutableHashSet<IChromosome> Execute(IPopulation population);
    }
}