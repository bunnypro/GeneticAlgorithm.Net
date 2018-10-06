using System;
using System.Timers;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Terminations
{
    public class TimeLimitTerminationCondition : ITerminationCondition
    {
        private readonly TimeSpan _timeLimit;

        public TimeLimitTerminationCondition(double milliseconds) : this(TimeSpan.FromMilliseconds(milliseconds))
        {
        }

        public TimeLimitTerminationCondition(TimeSpan timeLimit)
        {
            _timeLimit = timeLimit;
        }

        public bool Fulfilled(IEvolutionState state)
        {
            return state.EvolutionTime >= _timeLimit;
        }
    }
}
