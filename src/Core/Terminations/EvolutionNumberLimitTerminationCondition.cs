using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Terminations
{
    public class EvolutionNumberLimitTerminationCondition : ITerminationCondition
    {
        private readonly int _evolutionNumberLimit;

        public EvolutionNumberLimitTerminationCondition(int evolutionNumberLimit)
        {
            _evolutionNumberLimit = evolutionNumberLimit;
        }

        public bool Fulfilled(IEvolutionState state)
        {
            return state.EvolutionNumber >= _evolutionNumberLimit;
        }
    }
}