using Bunnypro.GeneticAlgorithm.Core.Populations;
using Bunnypro.GeneticAlgorithm.Examples.Simple;
using Bunnypro.GeneticAlgorithm.Standard;
using Bunnypro.GeneticAlgorithm.Standard.TestSuite;
using Xunit;

namespace Bunnypro.GeneticAlgorithm.Core.Test
{
    public class ElasticSizePopulationTest : PopulationStandardTest
    {
        private const int MinSize = 10;
        private const int MaxSize = 10;
        
        private static IPopulation CreatePopulation(int min, int max)
        {
            return new ElasticSizePopulation<SimpleChromosome>(min, max, new SimpleChromosomeFactory());
        }

        protected override IPopulation Population()
        {
            return CreatePopulation(MinSize, MaxSize);
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
                for (var i = 0; i < testLength; i++)
                {
                    population.Initialize();

                    Assert.True(population.Chromosomes.Count >= range.Min && population.Chromosomes.Count <= range.Max);
                }
            }
        }
    }
}