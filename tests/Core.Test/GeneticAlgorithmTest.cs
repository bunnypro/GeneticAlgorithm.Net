using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Bunnypro.GeneticAlgorithm.Core.Test
{
    public class GeneticAlgorithm
    {
        [Fact]
        public async Task Can_EvolveOnce()
        {
            var genetic = new Core.GeneticAlgorithm(CreatePopulation(10), CreateStrategy());
            var evolutionCount = genetic.States.EvolutionCount;
            await genetic.EvolveOnce();
            Assert.Equal(evolutionCount + 1, genetic.States.EvolutionCount);
        }

        [Fact]
        public async Task Can_Cancel_EvolveOnce()
        {
            using (var cts = new CancellationTokenSource())
            {
                var genetic = new Core.GeneticAlgorithm(CreatePopulation(10), CreateStrategy(delay: 1));
                Assert.False(genetic.States.IsCancelled);
                await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
                {
                    var evolution = genetic.EvolveOnce(cts.Token);
                    cts.Cancel();
                    await evolution;
                });
                Assert.True(genetic.States.IsCancelled);
            }

            using (var cts = new CancellationTokenSource())
            {
                var genetic = new Core.GeneticAlgorithm(CreatePopulation(10), CreateStrategy(delay: 500));
                await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
                {
                    var evolution1 = genetic.EvolveOnce();
                    var evolution2 = genetic.EvolveOnce(cts.Token);
                    await Task.Delay(100);
                    cts.Cancel();
                    await evolution2;
                    await evolution1;
                });
            }
        }

        [Fact]
        public async Task Can_Return_Evolution_Result_State()
        {
            const int delay = 500;
            const int accuracyError = 15; // system clock accuracy error (approximately)
            var genetic = new Core.GeneticAlgorithm(CreatePopulation(10), CreateStrategy(delay));
            var result = await genetic.EvolveOnce();
            Assert.Equal(1, result.EvolutionCount);
            Assert.True(result.EvolutionTime >= TimeSpan.FromMilliseconds(delay - accuracyError));
            Console.WriteLine(result.EvolutionTime);
            Assert.True(genetic.States.EvolutionCount >= result.EvolutionCount);
            Assert.True(genetic.States.EvolutionTime >= result.EvolutionTime);
        }

        private static IPopulation CreatePopulation(int count)
        {
            var populationMock = new Mock<IPopulation>();
            var isInitialized = false;
            populationMock.Setup(p => p.Chromosomes).Returns(CreateChromosome(count).ToImmutableHashSet());
            populationMock.Setup(p => p.IsInitialized).Returns(() => isInitialized);
            populationMock.Setup(p => p.Initialize()).Callback(() => isInitialized = true);
            return populationMock.Object;
        }

        private static IGeneticOperation CreateStrategy(int delay = 0)
        {
            var strategyMock = new Mock<IGeneticOperation>();
            strategyMock.Setup(o => o.Operate(It.IsAny<ImmutableHashSet<IChromosome>>(), It.IsAny<CancellationToken>()))
                .Returns<ImmutableHashSet<IChromosome>, CancellationToken>(async (chromosomes, token) =>
                {
                    await Task.Delay(delay, token);
                    return new HashSet<IChromosome>(chromosomes);
                });
            return strategyMock.Object;
        }

        private static HashSet<IChromosome> CreateChromosome(int count)
        {
            var chromosomes = new HashSet<IChromosome>();
            while (chromosomes.Count < count) chromosomes.Add(new Mock<IChromosome>().Object);
            return chromosomes;
        }
    }
}
