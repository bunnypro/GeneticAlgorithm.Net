using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Core.Populations;
using Bunnypro.GeneticAlgorithm.Core.Strategies;
using Bunnypro.GeneticAlgorithm.Standard;
using Moq;
using Moq.Protected;

namespace Bunnypro.GeneticAlgorithm.Core.Test
{
    public static class MockObject
    {
        public static IChromosomeFactory<T> ChromosomeFactory<T>(Func<T> create) where T : IChromosome
        {
            var factory = new Mock<IChromosomeFactory<T>>();
            factory.Setup(f => f.Create()).Returns(create);
            factory.Setup(f => f.Create(It.IsAny<int>())).Returns((int count) =>
            {
                var chromosomes = new HashSet<T>();
                while (chromosomes.Count < count) chromosomes.Add(factory.Object.Create());
                return chromosomes;
            });
            return factory.Object;
        }

        public static Population<T> Population<T>(HashSet<T> chromosomes) where T : IChromosome
        {
            var population = new Mock<Population<T>> {CallBase = true};
            population.Protected().Setup<ImmutableHashSet<T>>("CreateInitialChromosomes").Returns(chromosomes.ToImmutableHashSet());

            population.Protected()
                .Setup<ImmutableHashSet<T>>("FilterOffspring", chromosomes)
                .Returns((IEnumerable<T> offspring) => offspring.ToImmutableHashSet());

            return population.Object;
        }

        public static IEvolutionStrategy EvolutionStrategy()
        {
            var evolutionStrategy = new Mock<IEvolutionStrategy>();
            evolutionStrategy.Setup(e => e.Prepare(It.IsAny<IEnumerable<IChromosome>>()));
            evolutionStrategy.Setup(e => e.GenerateOffspring(It.IsAny<IEnumerable<IChromosome>>())).Returns((IEnumerable<IChromosome> parent) => parent);
            return evolutionStrategy.Object;
        }
    }
}