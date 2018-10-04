using System;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Core.Exceptions;
using Bunnypro.GeneticAlgorithm.Core.Termination;
using Bunnypro.GeneticAlgorithm.Examples.Simple;
using Bunnypro.GeneticAlgorithm.Standard;
using Xunit;

namespace Bunnypro.GeneticAlgorithm.Core.Test
{
    public class GeneticAlgorithmTest
    {
        private static IGeneticAlgorithm CreateGeneticAlgorithm()
        {
            return new GeneticAlgorithm(new SimplePopulation(), new SimpleStrategy());
        }

        [Fact]
        public async Task Can_not_be_reset_while_being_evolving()
        {
            var ga = CreateGeneticAlgorithm();
            var evolution = ga.Evolve();
            Assert.True(ga.Evolving);
            Assert.Throws<EvolutionRunningException>(() => { ga.Reset(); });
            Assert.False(((GeneticAlgorithm) ga).TryReset());
            ga.Stop();
            await evolution;
        }

        [Fact]
        public async Task Should_continue_evolve_after_stopped()
        {
            var ga = CreateGeneticAlgorithm();

            var evolving = ga.Evolve();
            await Task.Delay(1000);
            ga.Stop();
            await evolving;

            Assert.True(ga.EvolutionNumber > 0);
            var gn = ga.EvolutionNumber;

            var continued = ga.Evolve();
            await Task.Delay(1000);
            ga.Stop();
            await continued;

            Assert.True(ga.EvolutionNumber > gn);
        }

        [Fact]
        public async Task Should_continue_evolve_after_stopped_when_time_limit_not_exceed()
        {
            const double timeLimit = 1000;
            var ga = CreateGeneticAlgorithm();
            var evolving = ga.EvolveUntil(new TimeLimitTerminationCondition(timeLimit));
            await Task.Delay(200);
            ga.Stop();
            await evolving;
            Assert.True(ga.EvolutionNumber > 0);
            var gn = ga.EvolutionNumber;
            await ga.Evolve();
            Assert.True(ga.EvolutionNumber > gn);
        }

        [Fact]
        public async Task Should_continue_evolve_after_stopped_when_time_span_limit_not_exceed()
        {
            var ga = CreateGeneticAlgorithm();
            var evolving = ga.EvolveUntil(new TimeLimitTerminationCondition(new TimeSpan(0, 0, 1)));
            await Task.Delay(100);
            ga.Stop();
            await evolving;
            Assert.True(ga.EvolutionNumber > 0);
            var gn = ga.EvolutionNumber;
            await ga.Evolve();
            Assert.True(ga.EvolutionNumber > gn);
        }

        [Fact]
        public async Task Should_continue_evolve_until_condition_fulfilled_after_stopped()
        {
            const int maxGenerationNumber = 10;
            var ga = CreateGeneticAlgorithm();
            var evolution = ga.EvolveUntil(() =>
            {
                Task.Delay(500).Wait();
                return ga.EvolutionNumber >= maxGenerationNumber;
            });

            await Task.Delay(1000);
            ga.Stop();
            await evolution;

            Assert.True(maxGenerationNumber > ga.EvolutionNumber);
            await ga.Evolve();
            Assert.Equal(maxGenerationNumber, ga.EvolutionNumber);
        }

        [Fact]
        public async Task Should_evolve_until_condition_fulfilled()
        {
            const int maxGenerationNumber = 20;
            var ga = CreateGeneticAlgorithm();
            await ga.EvolveUntil(() => ga.EvolutionNumber >= maxGenerationNumber);

            Assert.Equal(maxGenerationNumber, ga.EvolutionNumber);

            const int nextMaxGenerationNumber = maxGenerationNumber + 10;
            await ga.EvolveUntil(() => ga.EvolutionNumber >= nextMaxGenerationNumber);

            Assert.Equal(nextMaxGenerationNumber, ga.EvolutionNumber);
        }

        [Fact]
        public async Task Should_evolve_until_stopped()
        {
            var ga = CreateGeneticAlgorithm();

            var evolving = ga.Evolve();
            await Task.Delay(2000);
            ga.Stop();
            await evolving;

            Assert.True(ga.EvolutionNumber > 0);
        }

        [Fact]
        public async Task Should_have_correct_evolving_state()
        {
            var ga = CreateGeneticAlgorithm();
            var evolving = ga.Evolve();
            await Task.Delay(100);
            Assert.True(ga.Evolving);
            ga.Stop();
            await evolving;
            Assert.False(ga.Evolving);
        }

        [Fact]
        public async Task Should_not_evolve_after_time_limit_expected()
        {
            const double timeLimit = 1000;
            var ga = CreateGeneticAlgorithm();
            await ga.EvolveUntil(new TimeLimitTerminationCondition(timeLimit));
            Assert.True(ga.EvolutionNumber > 0);
            var gn = ga.EvolutionNumber;
            await ga.Evolve();
            Assert.Equal(ga.EvolutionNumber, gn);
        }

        [Fact]
        public async Task Should_not_evolve_after_time_span_limit_expected()
        {
            var ga = CreateGeneticAlgorithm();
            await ga.EvolveUntil(new TimeLimitTerminationCondition(new TimeSpan(0, 0, 1)));
            Assert.True(ga.EvolutionNumber > 0);
            var gn = ga.EvolutionNumber;
            await ga.Evolve();
            Assert.Equal(ga.EvolutionNumber, gn);
        }

        [Fact]
        public async Task Should_not_evolve_if_condition_fulfilled()
        {
            const int maxGenerationNumber = 10;
            var ga = CreateGeneticAlgorithm();
            await ga.EvolveUntil(() => ga.EvolutionNumber >= maxGenerationNumber);

            Assert.Equal(maxGenerationNumber, ga.EvolutionNumber);
            await ga.Evolve();
            Assert.Equal(maxGenerationNumber, ga.EvolutionNumber);
        }

        [Fact]
        public async Task Should_not_evolve_if_termination_fulfilled()
        {
            const int maxGenerationNumber = 10;
            var ga = CreateGeneticAlgorithm();
            await ga.EvolveUntil(new FunctionTerminationCondition(() => ga.EvolutionNumber >= maxGenerationNumber));

            Assert.Equal(maxGenerationNumber, ga.EvolutionNumber);
            await ga.Evolve();
            Assert.Equal(maxGenerationNumber, ga.EvolutionNumber);
        }

        [Fact]
        public async Task Should_not_evolve_while_being_evolving()
        {
            var ga = CreateGeneticAlgorithm();
            var evolution = ga.Evolve();
            Assert.True(ga.Evolving);
            await Assert.ThrowsAsync<EvolutionRunningException>(async () => { await ga.Evolve(); });
            ga.Stop();
            await evolution;
        }
    }
}