using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bunnypro.GeneticAlgorithm.Core
{
    public interface IGeneticAlgorithm
    {
        IGeneticOperationStates States { get; }

        Task EvolveUntil(
            IPopulation population,
            GeneticOperationStates states,
            Func<IGeneticOperationStates, bool> termination,
            CancellationToken token);
    }
}
