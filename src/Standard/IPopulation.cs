using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IPopulation
    {
        void Initialize();
        void StoreOffspring(int generationNumber, ImmutableHashSet<IChromosome> offspring);
        void Reset();
    }
}