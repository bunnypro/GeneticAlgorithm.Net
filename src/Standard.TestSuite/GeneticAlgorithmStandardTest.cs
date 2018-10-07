using System;
using Xunit;

namespace Bunnypro.GeneticAlgorithm.Standard.TestSuite
{
    public abstract class GeneticAlgorithmStandardTest
    {
        /// <summary>
        /// This method returns a fresh instance implementation of IGeneticAlgorithm
        /// </summary>
        /// <returns></returns>
        protected abstract IGeneticAlgorithm GeneticAlgorithm();
        
        [Fact]
        public void Should_have_correct_initial_state()
        {
            var ga = GeneticAlgorithm();
            
            Assert.Equal(0, ga.State.EvolutionNumber);
            Assert.Equal(TimeSpan.Zero, ga.State.EvolutionTime);
            Assert.False(ga.State.Evolving);
        }
    }
}