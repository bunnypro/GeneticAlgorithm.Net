using System;
using Bunnypro.GeneticAlgorithm.Core.Chromosomes;
using Bunnypro.GeneticAlgorithm.Core.Populations.Generic;
using Bunnypro.GeneticAlgorithm.Standard;
using Bunnypro.GeneticAlgorithm.Standard.TestSuite;
using Xunit;

namespace Bunnypro.GeneticAlgorithm.Core.Test
{
    public class GenericFixedSizePopulationTest
    {
        private static FixedSizePopulation<Chromosome> CreatePopulation(int size)
        {
            return new FixedSizePopulation<Chromosome>(
                size, MockObject.ChromosomeFactory(() => new Chromosome(new object[] { new Random().Next(0, 100) }))
            );
        }

        [Fact]
        public void Should_initialize_with_correct_size()
        {
            const int size = 10;
            var population = CreatePopulation(size);
            Assert.Equal(size, population.Size);
            population.Initialize();
            Assert.Equal(size, population.Chromosomes.Count);
        }
    }
}
