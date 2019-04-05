using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Moq;

namespace Bunnypro.GeneticAlgorithm.TestUtils
{
    public static class MockingObject
    {
        public static IPopulation CreatePopulation(int count)
        {
            var populationMock = new Mock<IPopulation>();
            populationMock.Setup(p => p.Chromosomes).Returns(CreateChromosome(count).ToImmutableHashSet());
            return populationMock.Object;
        }

        public static IGeneticOperation CreateStrategy(int delay = 1)
        {
            var strategyMock = new Mock<IGeneticOperation>();
            strategyMock.Setup(o => o.Operate(It.IsAny<ImmutableHashSet<IChromosome>>(), It.IsAny<CancellationToken>()))
                .Returns<ImmutableHashSet<IChromosome>, CancellationToken>(async (chromosomes, token) =>
                {
                    await Task.Delay(delay, token);
                    return chromosomes;
                });
            return strategyMock.Object;
        }

        public static HashSet<IChromosome> CreateChromosome(int count)
        {
            var chromosomes = new HashSet<IChromosome>();
            while (chromosomes.Count < count) chromosomes.Add(new Mock<IChromosome>().Object);
            return chromosomes;
        }
    }
}
