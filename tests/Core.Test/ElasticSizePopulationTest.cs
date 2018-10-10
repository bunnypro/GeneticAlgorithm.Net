using System;
using Bunnypro.GeneticAlgorithm.Core.Chromosomes;
using Bunnypro.GeneticAlgorithm.Core.Populations;
using Bunnypro.GeneticAlgorithm.Standard;
using Bunnypro.GeneticAlgorithm.Standard.TestSuite;
using Xunit;

namespace Bunnypro.GeneticAlgorithm.Core.Test
{
    public class ElasticSizePopulationTest
    {
        private static ElasticSizePopulation<Chromosome> CreatePopulation(int min, int max)
        {
            return new ElasticSizePopulation<Chromosome>(
                min, max, MockObject.ChromosomeFactory(() => new Chromosome(new object[] {new Random().Next(0, 100)}))
            );
        }

        [Fact]
        public void Should_initialize_with_correct_size()
        {
            const int testLength = 10;

            var ranges = new[]
            {
                new {Min = 10, Max = 20},
                new {Min = 19, Max = 20},
                new {Min = 1, Max = 20}
            };

            foreach (var range in ranges)
            {
                var population = CreatePopulation(range.Min, range.Max);
                Assert.Equal(range.Min, population.MinSize);
                Assert.Equal(range.Max, population.MaxSize);
                for (var i = 0; i < testLength; i++)
                {
                    population.Initialize();
                    Assert.True(population.Chromosomes.Count >= range.Min && population.Chromosomes.Count <= range.Max);
                }
            }
        }
    }
}