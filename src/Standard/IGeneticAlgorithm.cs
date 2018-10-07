using System;
using System.Threading.Tasks;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IGeneticAlgorithm
    {
        IEvolutionState State { get; }
        IPopulation Population { get; }

        Task Evolve();
        Task Evolve(Func<IEvolutionState, bool> terminationCondition);
        Task Evolve(ITerminationCondition terminationCondition);
        void Stop();
        void Reset();
    }
}