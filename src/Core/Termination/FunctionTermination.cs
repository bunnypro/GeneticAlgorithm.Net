using System;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.Core.Termination
{
    public class FunctionTermination : ITermination
    {
        private readonly Func<bool> _fulfilled;

        public FunctionTermination(Func<bool> fulfilled)
        {
            _fulfilled = fulfilled;
        }

        public bool Fulfilled()
        {
            return _fulfilled();
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