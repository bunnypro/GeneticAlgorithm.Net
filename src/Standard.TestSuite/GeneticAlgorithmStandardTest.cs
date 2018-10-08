using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
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
        public async Task Should_have_correct_evolution_state_management()
        {
            var timeLimit = 50;
            var ga = GeneticAlgorithm();

            Assert.Equal(0, ga.State.EvolutionNumber);
            Assert.Equal(TimeSpan.Zero, ga.State.EvolutionTime);
            Assert.False(ga.State.Evolving);

            var evolution = ga.Evolve(state => state.EvolutionTime > TimeSpan.FromMilliseconds(timeLimit));
            while (!ga.State.Evolving)
            {
                timeLimit += 10;
                await Task.Delay(10);
            }

            Assert.True(ga.State.Evolving);

            while (ga.State.EvolutionNumber == 0)
            {
                await Task.Delay(1);
            }

            Assert.True(ga.State.EvolutionNumber > 0);
            Assert.True(ga.State.EvolutionTime > TimeSpan.Zero);

            await evolution;

            Assert.False(ga.State.Evolving);
        }

        [Fact]
        public async Task Should_have_population_chromosomes_after_evolving()
        {
            const int evolutionNumberLimit = 1;

            var ga = GeneticAlgorithm();
            Assert.Null(ga.Population.Chromosomes);
            var evolution = ga.Evolve(state => state.EvolutionNumber >= evolutionNumberLimit);
            while (ga.State.EvolutionNumber == 0)
            {
                await Task.Delay(1);
            }

            Assert.NotNull(ga.Population.Chromosomes);

            await evolution;

            Assert.NotNull(ga.Population.Chromosomes);
        }

        [Fact]
        public async Task Can_terminate_evolution_incorrect_condition()
        {
            var evolutionNumberLimits = new[]
            {
                1, 5, 10, 100, 1000
            };

            foreach (var evolutionNumberLimit in evolutionNumberLimits)
            {
                var ga = GeneticAlgorithm();
                await ga.Evolve(state => state.EvolutionNumber == evolutionNumberLimit);
                Assert.True(ga.State.EvolutionNumber == evolutionNumberLimit);
            }

            foreach (var evolutionNumberLimit in evolutionNumberLimits)
            {
                var terminationMock = new Mock<ITerminationCondition>();
                terminationMock.Setup(t => t.Fulfilled(It.IsAny<IEvolutionState>())).Returns((IEvolutionState state) => state.EvolutionNumber >= evolutionNumberLimit);
                
                var ga = GeneticAlgorithm();
                await ga.Evolve(terminationMock.Object);
                Assert.True(ga.State.EvolutionNumber == evolutionNumberLimit);
            }
        }
    }
}