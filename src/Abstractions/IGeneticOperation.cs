using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Bunnypro.GeneticAlgorithm.Abstractions
{
    public interface IGeneticOperation
    {
        Task<ImmutableHashSet<IChromosome>> Operate(ImmutableHashSet<IChromosome> chromosomes, CancellationToken token = default);
    }
}
