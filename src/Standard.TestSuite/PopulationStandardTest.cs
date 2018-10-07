using Xunit;

namespace Bunnypro.GeneticAlgorithm.Standard.TestSuite
{
    public partial class PopulationStandardTest
    {
        [Fact]
        public void Should_have_default_null_chromosomes()
        {
            var population = Population();
            Assert.Null(population.Chromosomes);
        }

        [Fact]
        public void Should_not_have_null_chromosomes_after_population_initialization()
        {
            var population = Population();
            population.Initialize();
            Assert.NotNull(population.Chromosomes);
        }

        [Fact]
        public void Should_have_null_chromosomes_after_population_reset()
        {
            var population = Population();
            population.Initialize();
            population.Reset();
            Assert.Null(population.Chromosomes);
        }

        [Fact]
        public void Can_store_offspring()
        {
            var population = Population();
            var dummy = Population();
            
            population.Initialize();
            dummy.Initialize();

            var initialChromosomes = population.Chromosomes;
            population.StoreOffspring(dummy.Chromosomes);
            
            Assert.NotNull(population.Chromosomes);
        }
    }
}