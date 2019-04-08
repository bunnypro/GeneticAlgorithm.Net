using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.Abstractions;
using Bunnypro.GeneticAlgorithm.Primitives;
using Moq;

namespace Bunnypro.GeneticAlgorithm.Test.Utils
{
    public static class MockObject
    {
        // system clock accuracy error (approximately)
        public const int SYSTEM_CLOCK_ACCURACY_ERROR = 15;

        public static TimeSpan CreateProximateAccuracyTimeSpan(int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds - SYSTEM_CLOCK_ACCURACY_ERROR);
        }

        public static IGeneticOperation CreateOperationStrategy(int delay = 1)
        {
            var mock = new Mock<IGeneticOperation>();
            mock.Setup(s => s.Operate(
                It.IsAny<ImmutableHashSet<IChromosome>>(),
                It.IsAny<PopulationCapacity>(),
                It.IsAny<CancellationToken>())
            ).Returns<ImmutableHashSet<IChromosome>, PopulationCapacity, CancellationToken>(async (c, s, t) =>
            {
                await Task.Delay(delay, t);
                return CreateChromosomes(s.Minimum).ToImmutableHashSet();
            });
            return mock.Object;
        }

        public static IPopulation CreatePopulation()
        {
            var mock = new Mock<IPopulation>();
            mock.SetupProperty(p => p.Chromosomes, null);
            return mock.Object;
        }

        public static HashSet<IChromosome> CreateChromosomes(int count)
        {
            var chromosomes = new HashSet<IChromosome>();
            while (chromosomes.Count < count) chromosomes.Add(new Mock<IChromosome>().Object);
            return chromosomes;
        }
    }
}