using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IGeneticAlgorithm
    {
        IReadOnlyGeneticOperationStates States { get; }

        Task EvolveUntil(
            IPopulation population,
            IGeneticOperationStates states,
            Func<IReadOnlyGeneticOperationStates, bool> termination,
            CancellationToken token);
    }
}
