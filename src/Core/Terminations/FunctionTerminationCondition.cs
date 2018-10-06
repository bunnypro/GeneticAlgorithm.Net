using System;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Terminations
{
    public class FunctionTerminationCondition : ITerminationCondition
    {
        private readonly Func<IEvolutionState, bool> _fulfilled;

        public FunctionTerminationCondition(Func<IEvolutionState, bool> fulfilled)
        {
            _fulfilled = fulfilled;
        }

        public bool Fulfilled(IEvolutionState state) => _fulfilled(state);
    }
}
