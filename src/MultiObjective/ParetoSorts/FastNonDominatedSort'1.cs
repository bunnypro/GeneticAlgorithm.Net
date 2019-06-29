using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.MultiObjective.Abstractions;
using Bunnypro.GeneticAlgorithm.MultiObjective.Primitives;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.ParetoSorts
{
    public class FastNonDominatedSort<T> : IParetoSort<T> where T : Enum
    {
        public IEnumerable<ImmutableArray<IChromosome<T>>> Sort(IEnumerable<IChromosome<T>> chromosomes)
        {
            var dominations = new Dictionary<IChromosome<T>, DominationData>();
            var frontBuilder = ImmutableArray.CreateBuilder<IChromosome<T>>();
            var c = chromosomes.ToList();
            for (var i = 0; i < c.Count; i++)
            {
                RegisterChromosome(c[i]);
                for (var j = i + 1; j < c.Count; j++)
                {
                    RegisterChromosome(c[j]);
                    var sign = Compare(c[i].ObjectiveValues, c[j].ObjectiveValues);
                    if (sign > 0)
                        Dominate(c[i], c[j]);
                    if (sign < 0)
                        Dominate(c[j], c[i]);
                }
                if (dominations[c[i]].DominatedCount == 0)
                    frontBuilder.Add(c[i]);
            }

            ImmutableArray<IChromosome<T>> previousFront;
            while (frontBuilder.Count > 0)
            {
                previousFront = frontBuilder.ToImmutable();
                yield return previousFront;
                frontBuilder.Clear();
                foreach (var dominatingChromosome in previousFront)
                {
                    foreach (var dominatedChromosome in dominations[dominatingChromosome].DominatedChromosomes)
                    {
                        var count = dominations[dominatedChromosome].DominatedCount--;
                        if (count == 0) frontBuilder.Add(dominatedChromosome);
                    }
                }
            }
            
            void RegisterChromosome(IChromosome<T> chromosome)
            {
                if (dominations.ContainsKey(chromosome)) return;
                dominations.Add(chromosome, new DominationData());
            }

            void Dominate(IChromosome<T> dominating, IChromosome<T> dominated)
            {
                dominations[dominating].DominatedChromosomes.Add(dominated);
                dominations[dominated].DominatedCount++;
            }
        }

        private static int Compare(ObjectiveValues<T> values, ObjectiveValues<T> otherValues)
        {
            var domination = 0;
            foreach (var objective in Enum.GetValues(typeof(T)).Cast<T>())
            {
                var sign = Math.Sign(values[objective].CompareTo(otherValues[objective]));
                if (sign == 0 || domination == sign)
                    continue;
                if (domination != 0)
                    return 0;
                domination = sign;
            }
            return domination;
        }
        
        private class DominationData
        {
            public IList<IChromosome<T>> DominatedChromosomes { get; } = new List<IChromosome<T>>();
            public int DominatedCount { get; set; }
        }
    }
}