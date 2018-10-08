using Xunit;

namespace Bunnypro.GeneticAlgorithm.Standard.TestSuite
{
    public abstract class PopulationStandardTest
    {
        protected abstract IPopulation Population();
        
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
        public void Can_store_offspring()
        {
            var population = Population();
            var dummy = Population();
            
            population.Initialize();
            dummy.Initialize();

            population.StoreOffspring(dummy.Chromosomes);
            
            Assert.NotNull(population.Chromosomes);
        }
    }
}