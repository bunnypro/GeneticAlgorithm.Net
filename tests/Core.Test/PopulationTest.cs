using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Bunnypro.GeneticAlgorithm.Core.Exceptions;
using Bunnypro.GeneticAlgorithm.Test.Utils;
using Xunit;

namespace Bunnypro.GeneticAlgorithm.Core.Test
{
    public class Population_Test
    {
        [Fact]
        public async Task Can_Increase_GenerationNumber_Every_Chromosomes_Stored()
        {
            var population = new Population();
            Assert.Null(population.Chromosomes);
            Assert.Equal(-1, population.GenerationNumber);
            population.Chromosomes = MockObject.CreateChromosomes(10).ToImmutableHashSet();
            Assert.Equal(0, population.GenerationNumber);

            IGeneticAlgorithm genetic = new GeneticAlgorithm(MockObject.CreateOperationStrategy());
            int number = population.GenerationNumber;
            var result = await genetic.EvolveUntil(population, s => s.EvolutionCount >= 5);
            Assert.Equal(result.EvolutionCount, population.GenerationNumber - number);
        }

        [Fact]
        public void Throw_When_Receive_Chromosomes_Out_Of_Capacity()
        {
            {
                var population = new Population();
                var number = population.GenerationNumber;
                Assert.Throws<SizeOutOfCapacityException>(() =>
                    population.Chromosomes = MockObject.CreateChromosomes(1).ToImmutableHashSet());
                Assert.Equal(number, population.GenerationNumber);
                population.Chromosomes = MockObject.CreateChromosomes(2).ToImmutableHashSet();
                Assert.Equal(number + 1, population.GenerationNumber);
            }
            {
                var population = new Population
                {
                    Capacity = new Core.PopulationCapacity(10)
                };
                var number = population.GenerationNumber;
                Assert.Throws<SizeOutOfCapacityException>(() =>
                    population.Chromosomes = MockObject.CreateChromosomes(9).ToImmutableHashSet());
                Assert.Equal(number, population.GenerationNumber);
                Assert.Throws<SizeOutOfCapacityException>(() =>
                    population.Chromosomes = MockObject.CreateChromosomes(11).ToImmutableHashSet());
                Assert.Equal(number, population.GenerationNumber);
                population.Chromosomes = MockObject.CreateChromosomes(10).ToImmutableHashSet();
                Assert.Equal(number + 1, population.GenerationNumber);
            }
            {
                var population = new Population
                {
                    Capacity = new Core.PopulationCapacity(5, 10)
                };
                var number = population.GenerationNumber;
                Assert.Throws<SizeOutOfCapacityException>(() =>
                    population.Chromosomes = MockObject.CreateChromosomes(4).ToImmutableHashSet());
                Assert.Equal(number, population.GenerationNumber);
                Assert.Throws<SizeOutOfCapacityException>(() =>
                    population.Chromosomes = MockObject.CreateChromosomes(11).ToImmutableHashSet());
                Assert.Equal(number, population.GenerationNumber);
                population.Chromosomes = MockObject.CreateChromosomes(7).ToImmutableHashSet();
                Assert.Equal(number + 1, population.GenerationNumber);
            }
        }
    }
}
