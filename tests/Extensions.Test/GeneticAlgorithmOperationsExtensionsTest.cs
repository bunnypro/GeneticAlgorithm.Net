using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Bunnypro.GeneticAlgorithm.Test.Utils;
using Xunit;

namespace Bunnypro.GeneticAlgorithm.Extensions.Test
{
    public class GeneticAlgorithmOperationsExtensions_Test
    {
        [Fact]
        public async Task Can_Evolve_Until_Cancelled()
        {
            const int time = 100;
            var population = MockObject.CreatePopulation();
            population.Chromosomes = MockObject.CreateChromosomes(10).ToImmutableHashSet();
            IGeneticAlgorithm genetic = new Core.GeneticAlgorithm(MockObject.CreateOperationStrategy());
            using (var cts = new CancellationTokenSource())
            {
                var evolution = genetic.Evolve(population, cts.Token);
                await Task.Delay(time);
                cts.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => evolution);
                Assert.True(cts.IsCancellationRequested);
            }
        }


        [Fact]
        public async Task Can_Evolve_Until_Cancelled_Whithout_Throwing()
        {
            const int time = 100;
            var population = MockObject.CreatePopulation();
            population.Chromosomes = MockObject.CreateChromosomes(10).ToImmutableHashSet();
            IGeneticAlgorithm genetic = new Core.GeneticAlgorithm(MockObject.CreateOperationStrategy());
            using (var cts = new CancellationTokenSource())
            {
                var evolution = genetic.TryEvolve(population, cts.Token);
                await Task.Delay(time);
                cts.Cancel();
                var result = await evolution;
                Assert.True(result.EvolutionTime >= MockObject.CreateProximateAccuracyTimeSpan(time));
                Assert.True(cts.IsCancellationRequested);
            }
        }
    }
}