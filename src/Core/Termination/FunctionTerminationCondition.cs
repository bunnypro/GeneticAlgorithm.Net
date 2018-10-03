using System;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Termination
{
    public class FunctionTerminationCondition : ITerminationCondition
    {
        private readonly Func<bool> _fulfilled;

        public bool Fulfilled => _fulfilled();

        public FunctionTerminationCondition(Func<bool> fulfilled)
        {
            _fulfilled = fulfilled;
        }

        public void Start()
        {
            // Method is Not Needed
        }

        public void Pause()
        {
            // Method is Not Needed
        }

        public void Reset()
        {
            // Method is Not Needed
        }
    }
}
