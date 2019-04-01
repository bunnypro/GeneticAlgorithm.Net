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
        // system clock accuracy error (approximately)
        private const int SYSTEM_CLOCK_ACCURACY_ERROR = 15;

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
                var genetic = new Core.GeneticAlgorithm(CreatePopulation(10), CreateStrategy(delay: 500));
                Assert.False(genetic.States.IsCanceled);
                await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
                {
                    var evolution = genetic.EvolveOnce(cts.Token);
                    cts.Cancel();
                    await evolution;
                });
                Assert.True(genetic.States.IsCanceled);
            }

            using (var cts = new CancellationTokenSource())
            {
                var genetic = new Core.GeneticAlgorithm(CreatePopulation(10), CreateStrategy(delay: 500));
                var evolution1 = genetic.EvolveOnce();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
                {
                    var evolution2 = genetic.EvolveOnce(cts.Token);
                    cts.Cancel();
                    await evolution2;
                });
                var result = await evolution1;
                Assert.True(result.EvolutionCount > 0);
                Assert.True(result.EvolutionTime > TimeSpan.Zero);
            }
        }

        [Fact]
        public async Task Can_Return_Evolution_Result_State()
        {
            const int delay = 500;
            var genetic = new Core.GeneticAlgorithm(CreatePopulation(10), CreateStrategy(delay));
            var result = await genetic.EvolveOnce();
            Assert.Equal(1, result.EvolutionCount);
            Assert.True(result.EvolutionTime >= TimeSpan.FromMilliseconds(delay - SYSTEM_CLOCK_ACCURACY_ERROR));
            Assert.True(genetic.States.EvolutionCount >= result.EvolutionCount);
            Assert.True(genetic.States.EvolutionTime >= result.EvolutionTime);
        }

        [Fact]
        public async Task Can_Handle_EvolutionTime_When_Canceled()
        {
            using (var cts = new CancellationTokenSource())
            {
                const int delay = 100;
                var genetic = new Core.GeneticAlgorithm(CreatePopulation(10), CreateStrategy(500));
                var time = genetic.States.EvolutionTime;
                await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
                {
                    var evolution = genetic.EvolveOnce(cts.Token);
                    await Task.Delay(delay);
                    cts.Cancel();
                    await evolution;
                });
                Assert.True(genetic.States.IsCanceled);
                Assert.True(genetic.States.EvolutionTime - time >= TimeSpan.FromMilliseconds(delay - SYSTEM_CLOCK_ACCURACY_ERROR));
            }
        }

        [Fact]
        public async Task Can_Evolve_Until_CancellationToken_Canceled()
        {
            const int cancellationDelay = 1000;
            var cancellationDelayError = TimeSpan.FromMilliseconds(cancellationDelay - SYSTEM_CLOCK_ACCURACY_ERROR);
            using (var cts = new CancellationTokenSource())
            {
                var genetic = new Core.GeneticAlgorithm(CreatePopulation(10), CreateStrategy());
                await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
                {
                    var evolution = genetic.EvolveUntil(cts.Token);
                    await Task.Delay(cancellationDelay);
                    cts.Cancel();
                    var result = await evolution;
                    Assert.True(genetic.States.IsCanceled);
                    Assert.True(result.EvolutionCount > 0);
                    Assert.True(result.EvolutionTime >= cancellationDelayError);
                    Assert.True(genetic.States.EvolutionCount >= result.EvolutionCount);
                    Assert.True(genetic.States.EvolutionTime >= result.EvolutionTime);
                });
            }
        }

        [Theory]
        [MemberData(nameof(GetTerminationConditionData))]
        public async Task Can_Evolve_Until_Termination_Condition_Fulfilled(
            Func<IGeneticAlgorithmCountedStates, bool> termination,
            Action<IGeneticAlgorithmStates, IGeneticAlgorithmCountedStates> assertion)
        {
            var genetic = new Core.GeneticAlgorithm(CreatePopulation(10), CreateStrategy());
            var result = await genetic.EvolveUntil(termination);
            assertion.Invoke(genetic.States, result);
        }

        public static IEnumerable<object[]> GetTerminationConditionData()
        {
            {
                const int count = 10;
                yield return new object[] {
                    (Func<IGeneticAlgorithmCountedStates, bool>) (states => states.EvolutionCount >= count),
                    (Action<IGeneticAlgorithmStates, IGeneticAlgorithmCountedStates>) (
                        (states, result) => {
                            Assert.True(result.EvolutionCount >= count);
                            Assert.True(states.EvolutionCount >= result.EvolutionCount);
                        }
                    )
                };
            }
            {
                const int time = 1000;
                var timeSpan = TimeSpan.FromMilliseconds(time);
                var timeSpanError = TimeSpan.FromMilliseconds(time - SYSTEM_CLOCK_ACCURACY_ERROR);
                yield return new object[] {
                    (Func<IGeneticAlgorithmCountedStates, bool>) (states => states.EvolutionTime >= timeSpan),
                    (Action<IGeneticAlgorithmStates, IGeneticAlgorithmCountedStates>) (
                        (states, result) => {
                            Assert.True(result.EvolutionTime >= timeSpanError);
                            Assert.True(states.EvolutionTime >= result.EvolutionTime);
                        }
                    )
                };
            }
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

        private static IGeneticOperation CreateStrategy(int delay = 1)
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
