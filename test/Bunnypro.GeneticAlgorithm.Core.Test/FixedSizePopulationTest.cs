using Bunnypro.GeneticAlgorithm.Core.Populations;
using Bunnypro.GeneticAlgorithm.Examples.Simple;
using Bunnypro.GeneticAlgorithm.Standard;
using Xunit;

namespace Bunnypro.GeneticAlgorithm.Core.Test
{
    public class FixedSizePopulationTest
    {
        private static IPopulation CreatePopulation(int size)
        {
            return new FixedSizePopulation<SimpleChromosome>(size, new SimpleChromosomeFactory());
        }

        [Fact]
        public void Should_initialize_with_correct_size()
        {
            const int size = 10;
            var population = CreatePopulation(size);
            population.Initialize();

            Assert.Equal(size, population.Chromosomes.Count);
        }
    }
}