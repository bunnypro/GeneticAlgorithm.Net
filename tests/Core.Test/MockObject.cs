using System.Collections.Generic;
using System.Collections.Immutable;
using Bunnypro.GeneticAlgorithm.Core.GeneticOperations;
using Bunnypro.GeneticAlgorithm.Core.Populations;
using Bunnypro.GeneticAlgorithm.Standard;
using Moq;
using Moq.Protected;

namespace Bunnypro.GeneticAlgorithm.Core.Test
{
    public static class MockObject
    {
        public static Population Population(HashSet<IChromosome> chromosomes)
        {
            var population = new Mock<Population> { CallBase = true };
            population.Protected().Setup<ImmutableHashSet<IChromosome>>("CreateInitialChromosomes").Returns(chromosomes.ToImmutableHashSet());

            population.Protected()
                .Setup<ImmutableHashSet<IChromosome>>("FilterOffspring", chromosomes)
                .Returns((IEnumerable<IChromosome> offspring) => offspring.ToImmutableHashSet());

            return population.Object;
        }

        public static IPreparableOperation EvolutionStrategy()
        {
            var evolutionStrategy = new Mock<IPreparableOperation>();
            evolutionStrategy.Setup(e => e.Prepare(It.IsAny<IEnumerable<IChromosome>>()));
            evolutionStrategy.Setup(e => e.Operate(It.IsAny<IEnumerable<IChromosome>>(), It.IsAny<int>())).Returns((IEnumerable<IChromosome> parent, int size) => parent);
            return evolutionStrategy.Object;
        }
    }
}
