using System;
using System.Threading.Tasks;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IGeneticAlgorithm
    {
        IEvolutionState State { get; }

        Task Evolve();
        Task EvolveUntil(Func<IEvolutionState, bool> fulfilled);
        Task EvolveUntil(ITerminationCondition terminationCondition);
        void Stop();
        void Reset();
    }
}
