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
        public static IGeneticAlgorithm CreateGeneticAlgorithm(int delay = 1)
        {
            var geneticMock = new Mock<IGeneticAlgorithm>();
            var states = CreateStates();
            geneticMock.Setup(g => g.States).Returns(() => states);
            geneticMock.Setup(g => g.EvolveUntil(
                It.IsAny<IPopulation>(),
                It.IsAny<IGeneticOperationStates>(),
                It.IsAny<Func<IReadOnlyGeneticOperationStates, bool>>(),
                It.IsAny<CancellationToken>()
            )).Returns<IPopulation, IGeneticOperationStates, Func<IReadOnlyGeneticOperationStates, bool>, CancellationToken>(async (p, s, f, t) =>
            {
                while (!f.Invoke(s))
                {
                    var result = new FakeGeneticOperationState();
                    var startTime = DateTime.Now;
                    try
                    {
                        await Task.Delay(delay, t);
                        p.Chromosomes = MockingObject.CreateChromosome(10).ToImmutableHashSet();
                        result.EvolutionCount++;
                    }
                    finally
                    {
                        result.EvolutionTime += DateTime.Now - startTime;
                        lock (states) states.Extend(result);
                        s.Extend(result);
                    }
                }
            });
            return geneticMock.Object;
        }

        public static IGeneticOperationStates CreateStates()
        {
            return new FakeGeneticOperationState();
        }

        public static IPopulation CreatePopulation()
        {
            var populationMock = new Mock<IPopulation>();
            ImmutableHashSet<IChromosome> chromosomes = null;
            populationMock.SetupGet(p => p.Chromosomes).Returns(() => chromosomes);
            populationMock.SetupSet(p => p.Chromosomes = It.IsAny<ImmutableHashSet<IChromosome>>())
                .Callback<ImmutableHashSet<IChromosome>>(value => chromosomes = value);
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
