using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Bunnypro.GeneticAlgorithm.Core;
using Bunnypro.GeneticAlgorithm.TestUtils;
using Moq;
using Xunit;

namespace Bunnypro.GeneticAlgorithm.Extensions.Test
{
    public class GeneticAlgorithmOperationExtensions_Test
    {
        // system clock accuracy error (approximately)
        private const int SystemClockAccuracyError = 15;

        [Fact]
        public async Task Can_Handle_Operation_Result_When_Canceled()
        {
            using (var cts = new CancellationTokenSource())
            {
                const int delay = 100;
                var population = MockingObject.CreatePopulation(10);
                var result = new GeneticOperationStates();
                var genetic = new Core.GeneticAlgorithm(MockingObject.CreateStrategy(delay: 500));
                var evolution = genetic.TryEvolveOnce(population, result, cts.Token);
                await Task.Delay(delay);
                cts.Cancel();
                Assert.False(await evolution);
                Assert.True(result.EvolutionTime >= TimeSpan.FromMilliseconds(delay - SystemClockAccuracyError));
            }
        }

        [Fact]
        public async Task Can_Continuing_Evolve_Until_Canceled()
        {
            const int time = 500;
            using (var cts = new CancellationTokenSource())
            {
                var genetic = new Core.GeneticAlgorithm(MockingObject.CreateStrategy());
                var population = MockingObject.CreatePopulation(10);
                var states = GeneticOperationStates.From(genetic.States);
                var result = new GeneticOperationStates();
                var evolution = genetic.Evolve(population, result, cts.Token);
                await Task.Delay(time);
                cts.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => evolution);
                Assert.True(result.EvolutionCount > 0);
                Assert.True(result.EvolutionTime >= TimeSpan.FromMilliseconds(time - SystemClockAccuracyError));
                Assert.True(genetic.States.EvolutionCount - states.EvolutionCount >= result.EvolutionCount);
                Assert.True(genetic.States.EvolutionTime - states.EvolutionTime >= result.EvolutionTime);
            }
        }

        [Fact]
        public async Task Can_Continuing_Evolve_Until_Canceled_Without_Throwing()
        {
            const int time = 500;
            using (var cts = new CancellationTokenSource())
            {
                var genetic = new Core.GeneticAlgorithm(MockingObject.CreateStrategy());
                var population = MockingObject.CreatePopulation(10);
                var states = GeneticOperationStates.From(genetic.States);
                var result = new GeneticOperationStates();
                var evolution = genetic.TryEvolve(population, result, cts.Token);
                await Task.Delay(time);
                cts.Cancel();
                Assert.False(await evolution);
                Assert.True(result.EvolutionCount > 0);
                Assert.True(result.EvolutionTime >= TimeSpan.FromMilliseconds(time - SystemClockAccuracyError));
                Assert.True(genetic.States.EvolutionCount - states.EvolutionCount >= result.EvolutionCount);
                Assert.True(genetic.States.EvolutionTime - states.EvolutionTime >= result.EvolutionTime);
            }
        }

        [Fact]
        public async Task Can_Continuing_Evolve_Until_Canceled_Or_Termination_Callback_Fulfilled_Without_throwing()
        {
            using (var cts = new CancellationTokenSource())
            {
                const int time = 500;
                const int delay = 100;
                bool Termination(IReadOnlyGeneticOperationStates states)
                {
                    return states.EvolutionTime >= TimeSpan.FromMilliseconds(time);
                }
                var genetic = new Core.GeneticAlgorithm(MockingObject.CreateStrategy());
                var population = MockingObject.CreatePopulation(10);
                {
                    var result = new GeneticOperationStates();
                    var evolution = genetic.TryEvolveUntil(population, result, Termination, cts.Token);
                    await Task.Delay(delay);
                    cts.Cancel();
                    Assert.False(await evolution);
                    Assert.True(result.EvolutionTime >= TimeSpan.FromMilliseconds(delay - SystemClockAccuracyError));
                    Assert.True(genetic.States.EvolutionTime >= result.EvolutionTime);
                }
                {
                    var result = new GeneticOperationStates();
                    Assert.True(await genetic.TryEvolveUntil(population, result, Termination, default));
                    Assert.True(result.EvolutionTime >= TimeSpan.FromMilliseconds(time - SystemClockAccuracyError));
                    Assert.True(genetic.States.EvolutionTime >= result.EvolutionTime);
                }
            }
        }
    }
}
