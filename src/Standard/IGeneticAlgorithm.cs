using System;
using System.Threading.Tasks;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IGeneticAlgorithm
    {
        int EvolutionNumber { get; }
        bool Evolving { get; }

        Task Evolve();
        Task EvolveUntil(Func<bool> fulfilled);
        Task EvolveUntil(ITerminationCondition terminationCondition);
        void Stop();
        void Reset();
    }
}