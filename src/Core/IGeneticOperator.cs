using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IGeneticOperator
    {
        Task<HashSet<IChromosome>> Operate(ImmutableHashSet<IChromosome> chromosomes, CancellationToken token = default);
    }
}