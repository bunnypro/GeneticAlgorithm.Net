using System;
using System.Threading.Tasks;
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

        [Fact]
        public async Task Can_evolve_and_stop_and_also_have_correct_evolving_state()
        {
            var ga = GeneticAlgorithm();

            var evolving = ga.Evolve();
            Assert.True(ga.State.Evolving);
            ga.Stop();
            await evolving;
            Assert.False(ga.State.Evolving);
        }
        
        [Fact]
        public async Task Should_have_correct_state_after_evolving()
        {
            var ga = GeneticAlgorithm();
            
            await ga.EvolveUntil(state => state.EvolutionNumber >= 1);
            Assert.Equal(1, ga.State.EvolutionNumber);
            Assert.True(ga.State.EvolutionTime > TimeSpan.Zero);
            Assert.False(ga.State.Evolving);
        }
        
        [Fact]
        public async Task Can_be_reset_and_have_correct_state_after_reset()
        {
            var ga = GeneticAlgorithm();
            
            await ga.EvolveUntil(state => state.EvolutionNumber >= 1);
            ga.Reset();
            
            Assert.Equal(0, ga.State.EvolutionNumber);
            Assert.Equal(TimeSpan.Zero, ga.State.EvolutionTime);
            Assert.False(ga.State.Evolving);
        }
    }
}