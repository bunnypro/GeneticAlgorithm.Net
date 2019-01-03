using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.NonDominatedSorting
{
    public class PerformantNonDominatedSorting<T> : INonDominatedSorting<T> where T : IChromosome
    {
        public IEnumerable<ImmutableArray<T>> Sort(IEnumerable<T> chromosomes)
        {
            var dataSet = chromosomes.ToArray();
            var dataHolder = new Dictionary<T, DataHolder>();
            var firstFrontBuilder = ImmutableArray.CreateBuilder<T>();

            void RegisterHolder(T chromosome)
            {
                dataHolder.Add(chromosome, new DataHolder {DominationList = new List<T>(), DominatedCount = 0});
            }

            void Dominate(T dominating, T dominated)
            {
                dataHolder[dominating].DominationList.Add(dominated);
                dataHolder[dominated].DominatedCount++;
            }

            for (var x = 0; x < dataSet.Length; x++)
            {
                var chromosome = dataSet[x];
                if (x == 0)
                    RegisterHolder(chromosome);
                for (var y = x + 1; y < dataSet.Length; y++)
                {
                    var otherChromosome = dataSet[y];
                    if (x == 0)
                        RegisterHolder(otherChromosome);
                    var dominateComparison = chromosome.Fitness.CompareTo(otherChromosome.Fitness);
                    if (dominateComparison == 0) continue;
                    if (dominateComparison > 0)
                        Dominate(chromosome, otherChromosome);
                    else
                        Dominate(otherChromosome, chromosome);
                }

                if (dataHolder[chromosome].DominatedCount == 0)
                    firstFrontBuilder.Add(chromosome);
            }

            var previousFront = firstFrontBuilder.ToImmutable();
            yield return previousFront;

            while (previousFront.Length > 0)
            {
                var frontBuilder = ImmutableArray.CreateBuilder<T>();
                foreach (var chromosome in previousFront)
                {
                    foreach (var dominatedChromosome in dataHolder[chromosome].DominationList)
                    {
                        dataHolder[dominatedChromosome].DominatedCount--;
                        if (dataHolder[dominatedChromosome].DominatedCount == 0)
                            frontBuilder.Add(dominatedChromosome);
                    }
                }

                if (frontBuilder.Count == 0)
                    yield break;

                previousFront = frontBuilder.ToImmutable();
                yield return previousFront;
            }
        }

        private class DataHolder
        {
            public IList<T> DominationList { get; set; }
            public int DominatedCount { get; set; }
        }
    }
}