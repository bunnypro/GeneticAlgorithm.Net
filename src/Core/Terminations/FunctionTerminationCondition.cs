using System;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Terminations
{
    public class FunctionTerminationCondition : ITerminationCondition
    {
        private readonly Func<IEvolutionState, bool> _function;

        public FunctionTerminationCondition(Func<IEvolutionState, bool> function)
        {
            _function = function;
        }

        public bool Fulfilled(IEvolutionState state)
        {
            return _function(state);
        }
    }
}