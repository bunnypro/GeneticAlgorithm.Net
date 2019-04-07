using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Bunnypro.GeneticAlgorithm.Core;
using Bunnypro.GeneticAlgorithm.Test.Utils;
using System.Collections.Immutable;

namespace Bunnypro.GeneticAlgorithm.Core.Test
{
    public class GeneticAlgorithm_Test
    {
        [Fact]
        public async Task Can_Evolve_Until_Termination_Condition_Fulfilled()
        {
            IGeneticOperation strategy = MockObject.CreateOperationStrategy();
            IPopulation population = MockObject.CreatePopulation();
            population.Chromosomes = MockObject.CreateChromosomes(10).ToImmutableHashSet();
            IGeneticAlgorithm genetic = new GeneticAlgorithm(strategy);
            {
                const int count = 10;
                var preEvolutionCount = genetic.EvolutionStates.EvolutionCount;
                IGeneticEvolutionStates result = await genetic.EvolveUntil(population, s => s.EvolutionCount >= count);
                Assert.True(genetic.EvolutionStates.EvolutionCount > preEvolutionCount);
                Assert.True(result.EvolutionCount >= count);
            }
            {
                var time = TimeSpan.FromMilliseconds(100);
                var preEvolutionTime = genetic.EvolutionStates.EvolutionTime;
                IGeneticEvolutionStates result = await genetic.EvolveUntil(population, s => s.EvolutionTime >= time);
                Assert.True(genetic.EvolutionStates.EvolutionTime > preEvolutionTime);
                Assert.True(result.EvolutionTime >= time);
            }
        }

        [Fact]
        public async Task Can_Evolve_Until_Termination_Condition_Fulfilled_Or_Canceled()
        {
            const int delay = 100;
            IGeneticOperation strategy = MockObject.CreateOperationStrategy();
            IPopulation population = MockObject.CreatePopulation();
            population.Chromosomes = MockObject.CreateChromosomes(10).ToImmutableHashSet();
            IGeneticAlgorithm genetic = new GeneticAlgorithm(strategy);
            using (var cts = new CancellationTokenSource())
            {
                var preEvolutionStates = genetic.EvolutionStates;
                var evolution = genetic.EvolveUntil(population, _ => false, cts.Token);
                await Task.Delay(delay);
                cts.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => evolution);
                var afterEvolutionStates = genetic.EvolutionStates;
                Assert.True(afterEvolutionStates.EvolutionCount > preEvolutionStates.EvolutionCount);
                Assert.True(afterEvolutionStates.EvolutionTime > preEvolutionStates.EvolutionTime);
                var differentTime = afterEvolutionStates.EvolutionTime - preEvolutionStates.EvolutionTime;
                Assert.True(differentTime >= MockObject.CreateProximateAccuracyTimeSpan(delay));
            }
            using (var cts = new CancellationTokenSource())
            {
                const int time = 50;
                var evolution = genetic.EvolveUntil(population, s => s.EvolutionTime >= TimeSpan.FromMilliseconds(time), cts.Token);
                await Task.Delay(delay);
                cts.Cancel();
                var result = await evolution;
                Assert.True(result.EvolutionTime >= MockObject.CreateProximateAccuracyTimeSpan(time));
            }
        }

        [Fact]
        public async Task Can_Evolve_Until_Termination_Condition_Fulfilled_Or_Canceled_Without_Throwing()
        {
            const int delay = 100;
            IGeneticOperation strategy = MockObject.CreateOperationStrategy();
            IPopulation population = MockObject.CreatePopulation();
            population.Chromosomes = MockObject.CreateChromosomes(10).ToImmutableHashSet();
            IGeneticAlgorithm genetic = new GeneticAlgorithm(strategy);
            using (var cts = new CancellationTokenSource())
            {
                var preEvolutionStates = genetic.EvolutionStates;
                Task<(IGeneticEvolutionStates, bool)> evolution = genetic.TryEvolveUntil(population, _ => false, cts.Token);
                await Task.Delay(delay);
                cts.Cancel();
                var (result, succeed) = await evolution;
                Assert.False(succeed);
                var afterEvolutionStates = genetic.EvolutionStates;
                Assert.True(afterEvolutionStates.EvolutionCount > preEvolutionStates.EvolutionCount);
                Assert.True(afterEvolutionStates.EvolutionTime > preEvolutionStates.EvolutionTime);
                var differentTime = afterEvolutionStates.EvolutionTime - preEvolutionStates.EvolutionTime;
                Assert.True(differentTime >= MockObject.CreateProximateAccuracyTimeSpan(delay));
                Assert.Equal(afterEvolutionStates.EvolutionCount - preEvolutionStates.EvolutionCount, result.EvolutionCount);
                Assert.Equal(afterEvolutionStates.EvolutionTime - preEvolutionStates.EvolutionTime, result.EvolutionTime);
            }
            using (var cts = new CancellationTokenSource())
            {
                const int time = 50;
                Task<(IGeneticEvolutionStates, bool)> evolution = genetic.TryEvolveUntil(
                    population,
                    s => s.EvolutionTime >= TimeSpan.FromMilliseconds(time),
                    cts.Token);
                await Task.Delay(delay);
                var (result, succeed) = await evolution;
                Assert.True(succeed);
                Assert.True(result.EvolutionTime >= MockObject.CreateProximateAccuracyTimeSpan(time));
            }
        }
    }
}
