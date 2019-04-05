using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Bunnypro.GeneticAlgorithm.TestUtils;

namespace Bunnypro.GeneticAlgorithm.Core.Test
{
    public class GeneticAlgorithm_Test
    {
        // system clock accuracy error (approximately)
        private const int SystemClockAccuracyError = 15;

        [Fact]
        public async Task Can_EvolveOnce()
        {
            var population = MockingObject.CreatePopulation(10);
            var genetic = new GeneticAlgorithm(MockingObject.CreateStrategy());
            var evolutionCount = genetic.States.EvolutionCount;
            var result = await genetic.EvolveOnce(population);
            Assert.Equal(evolutionCount + 1, genetic.States.EvolutionCount);
            Assert.Equal(1, result.EvolutionCount);
        }

        [Fact]
        public async Task Can_Cancel_EvolveOnce()
        {
            using (var cts = new CancellationTokenSource())
            {
                var population = MockingObject.CreatePopulation(10);
                var genetic = new GeneticAlgorithm(MockingObject.CreateStrategy(delay: 500));
                var evolution = genetic.EvolveOnce(population, cts.Token);
                cts.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => evolution);
            }
        }

        [Fact]
        public async Task Can_EvolveOnce_Without_Interruption_By_Another_Evolution_Cancellation()
        {
            using (var cts = new CancellationTokenSource())
            {
                var population = MockingObject.CreatePopulation(10);
                var genetic = new GeneticAlgorithm(MockingObject.CreateStrategy(delay: 500));
                var evolution1 = genetic.EvolveOnce(population);
                var evolution2 = genetic.EvolveOnce(population, cts.Token);
                cts.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => evolution2);
                var result = await evolution1;
                Assert.True(result.EvolutionCount > 0);
                Assert.True(result.EvolutionTime > TimeSpan.Zero);
            }
        }

        [Fact]
        public async Task Can_Handle_Internal_Operation_States_When_Canceled()
        {
            using (var cts = new CancellationTokenSource())
            {
                const int delay = 100;
                var population = MockingObject.CreatePopulation(10);
                var genetic = new GeneticAlgorithm(MockingObject.CreateStrategy(delay: 500));
                var time = genetic.States.EvolutionTime;
                var evolution = genetic.EvolveOnce(population, cts.Token);
                await Task.Delay(delay);
                cts.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => evolution);
                Assert.True(genetic.States.EvolutionTime - time >= TimeSpan.FromMilliseconds(delay - SystemClockAccuracyError));
            }
        }

        [Fact]
        public async Task Can_Return_Operation_Result_State()
        {
            const int delay = 500;
            var population = MockingObject.CreatePopulation(10);
            var genetic = new GeneticAlgorithm(MockingObject.CreateStrategy(delay));
            var result = await genetic.EvolveOnce(population);
            Assert.Equal(1, result.EvolutionCount);
            Assert.True(result.EvolutionTime >= TimeSpan.FromMilliseconds(delay - SystemClockAccuracyError));
            Assert.True(genetic.States.EvolutionCount >= result.EvolutionCount);
            Assert.True(genetic.States.EvolutionTime >= result.EvolutionTime);
        }

        [Theory]
        [MemberData(nameof(GetTerminationCallbackData))]
        public async Task Can_Continuing_Evolve_Until_Termination_Callback_Fulfilled(
            Func<IReadOnlyGeneticOperationStates, bool> termination,
            Action<IReadOnlyGeneticOperationStates, IReadOnlyGeneticOperationStates> assertion)
        {
            var genetic = new GeneticAlgorithm(MockingObject.CreateStrategy());
            var population = MockingObject.CreatePopulation(10);
            var result = await genetic.EvolveUntil(population, termination);
            assertion.Invoke(genetic.States, result);
        }

        [Fact]
        public async Task Can_Continuing_Evolve_Until_Canceled_Or_Termination_Callback_Fulfilled()
        {
            using (var cts = new CancellationTokenSource())
            {
                const int time = 500;
                const int delay = 100;
                bool Termination(IReadOnlyGeneticOperationStates states)
                {
                    return states.EvolutionTime >= TimeSpan.FromMilliseconds(time);
                }
                var genetic = new GeneticAlgorithm(MockingObject.CreateStrategy());
                var population = MockingObject.CreatePopulation(10);
                {
                    var result = new GeneticOperationStates();
                    var evolution = genetic.EvolveUntil(population, result, Termination, cts.Token);
                    await Task.Delay(delay);
                    cts.Cancel();
                    await Assert.ThrowsAnyAsync<OperationCanceledException>(() => evolution);
                    Assert.True(result.EvolutionTime >= TimeSpan.FromMilliseconds(delay - SystemClockAccuracyError));
                    Assert.True(genetic.States.EvolutionTime >= result.EvolutionTime);
                }
                {
                    var result = new GeneticOperationStates();
                    await genetic.EvolveUntil(population, result, Termination, default);
                    Assert.True(result.EvolutionTime >= TimeSpan.FromMilliseconds(time - SystemClockAccuracyError));
                    Assert.True(genetic.States.EvolutionTime >= result.EvolutionTime);
                }
            }
        }

        public static IEnumerable<object[]> GetTerminationCallbackData()
        {
            {
                const int count = 10;
                yield return new object[]
                {
                    (Func<IReadOnlyGeneticOperationStates, bool>) (states => states.EvolutionCount >= count),
                    (Action<IReadOnlyGeneticOperationStates, IReadOnlyGeneticOperationStates>) ((states, result) =>
                    {
                        Assert.Equal(count, result.EvolutionCount);
                        Assert.True(states.EvolutionCount >= result.EvolutionCount);
                    })
                };
            }

            {
                const int time = 100;
                yield return new object[]
                {
                    (Func<IReadOnlyGeneticOperationStates, bool>) (states => states.EvolutionTime >= TimeSpan.FromMilliseconds(time)),
                    (Action<IReadOnlyGeneticOperationStates, IReadOnlyGeneticOperationStates>) ((states, result) =>
                    {
                        Assert.True(result.EvolutionTime >= TimeSpan.FromMilliseconds(time - SystemClockAccuracyError));
                        Assert.True(states.EvolutionTime >= result.EvolutionTime);
                    })
                };
            }
        }
    }
}
