using System;
using Xunit;

namespace Bunnypro.GeneticAlgorithm.Standard.TestSuite
{
    public partial class GeneticAlgorithmStandardTest
    {
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