using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Populations
{
    public interface IChromosomeFactory
    {
        IChromosome Create();
        ImmutableHashSet<IChromosome> Create(int count);
    }
}