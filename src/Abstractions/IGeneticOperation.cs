using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Primitives;

namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IGeneticOperation
    {
        Task<ImmutableHashSet<IChromosome>> Operate(
            ImmutableHashSet<IChromosome> chromosomes,
            PopulationCapacity capacity,
            CancellationToken token = default);
    }
}