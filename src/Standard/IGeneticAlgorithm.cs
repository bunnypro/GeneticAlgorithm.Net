using System;
using System.Threading.Tasks;

namespace Bunnypro.GeneticAlgorithm.Standard
{
    public interface IGeneticAlgorithm
    {
        IEvolutionState State { get; }
        IPopulation Population { get; }
        IEvolutionStrategy EvolutionStrategy { get; }

        Task Evolve(Func<IEvolutionState, bool> terminationCondition);
        Task Evolve(ITerminationCondition terminationCondition);
    }
}