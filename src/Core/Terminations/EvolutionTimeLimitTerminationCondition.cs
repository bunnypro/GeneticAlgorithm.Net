using System;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Terminations
{
    public class EvolutionTimeLimitTerminationCondition : ITerminationCondition
    {
        private readonly TimeSpan _timeLimit;

        public EvolutionTimeLimitTerminationCondition(double milliseconds) : this(TimeSpan.FromMilliseconds(milliseconds))
        {
        }

        public EvolutionTimeLimitTerminationCondition(TimeSpan timeLimit)
        {
            _timeLimit = timeLimit;
        }

        public bool Fulfilled(IEvolutionState state)
        {
            return state.EvolutionTime >= _timeLimit;
        }
    }
}