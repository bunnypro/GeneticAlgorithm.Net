using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Bunnypro.GeneticAlgorithm.Core.Test
{
    public class GeneticAlgorithm
    {
        [Fact]
        public async Task Can_Evolve_Once()
        {
            var genetic = new Core.GeneticAlgorithm(CreatePopulation(10), CreateStrategy());
            var evolutionCount = genetic.States.EvolutionCount;
            var generationNumber = genetic.Population?.GenerationNumber ?? 0;
            await genetic.EvolveOnce();
            Assert.Equal(evolutionCount + 1, genetic.States.EvolutionCount);
            Assert.True(genetic.Population.GenerationNumber > generationNumber);
        }

        private static IPopulation CreatePopulation(int count)
        {
            var populationMock = new Mock<IPopulation>();
            ImmutableHashSet<IChromosome> currentChromosome = null;
            var isInitialized = false;
            var generationNumber = -1;
            populationMock.Setup(p => p.Chromosomes).Returns(() => currentChromosome);
            populationMock.Setup(p => p.IsInitialized).Returns(() => isInitialized);
            populationMock.Setup(p => p.GenerationNumber).Returns(() => generationNumber);
            populationMock.Setup(p => p.Initialize()).Callback(() =>
            {
                if (isInitialized) return;
                currentChromosome = CreateChromosome(count).ToImmutableHashSet();
                isInitialized = true;
                generationNumber = 0;
            });
            populationMock.Setup(p => p.RegisterOffspring(It.IsAny<HashSet<IChromosome>>()))
                .Callback<HashSet<IChromosome>>(chromosomes =>
                {
                    currentChromosome = chromosomes.ToImmutableHashSet();
                    generationNumber++;
                });
            return populationMock.Object;
        }

        private static IGeneticOperator CreateStrategy(int delay = 0)
        {
            var strategyMock = new Mock<IGeneticOperator>();
            strategyMock.Setup(o => o.Operate(It.IsAny<ImmutableHashSet<IChromosome>>(), new CancellationToken()))
                .Returns<ImmutableHashSet<IChromosome>, CancellationToken>(async (chromosomes, token) =>
                {
                    await Task.Delay(delay, token);
                    return new HashSet<IChromosome>(chromosomes);
                });
            return strategyMock.Object;
        }

        private static HashSet<IChromosome> CreateChromosome(int count)
        {
            IChromosome CreateChromosome() => new Mock<IChromosome>().Object;
            var chromosomes = new HashSet<IChromosome>();
            while (chromosomes.Count < count) chromosomes.Add(CreateChromosome());
            return chromosomes;
        }
    }
}