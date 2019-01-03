using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.NonDominatedSorting
{
    public class Nsga2<T> : INonDominatedSorting<T> where T : IChromosome
    {
        public IEnumerable<ImmutableArray<T>> Sort(IEnumerable<T> chromosomes)
        {
            var dataSet = chromosomes.ToArray();
            var fronts = new SortedDictionary<int, List<T>>();
            var dominationList = new Dictionary<T, List<T>>();
            var dominatedCount = new Dictionary<T, int>();

            fronts.Add(0, new List<T>());

            void RegisterDominationList(T p)
            {
                if (!dominationList.ContainsKey(p))
                    dominationList.Add(p, new List<T>());
            }
            void RegisterDominatedCount(T p)
            {
                if (!dominatedCount.ContainsKey(p))
                    dominatedCount.Add(p, 0);
            }
            void AddToDominationList(T p, T q)
            {
                RegisterDominationList(p);
                dominationList[p].Add(q);
            }
            void IncreaseDominatedCount(T p)
            {
                RegisterDominatedCount(p);
                dominatedCount[p]++;
            }

            for (var x = 0; x < dataSet.Length; x++)
            {
                var p = dataSet[x];
                RegisterDominationList(p);
                RegisterDominatedCount(p);
                for (var y = x + 1; y < dataSet.Length; y++)
                {
                    var q = dataSet[y];
                    var dominateState = p.Fitness.CompareTo(q.Fitness);
                    
                    if (dominateState > 0)
                    {
                        AddToDominationList(p, q);
                        IncreaseDominatedCount(q);
                    }
                    else if (dominateState < 0)
                    {
                        AddToDominationList(q, p);
                        IncreaseDominatedCount(p);
                    }
                }

                if (dominatedCount[p] == 0)
                {
                    fronts[0].Add(p);
                }
            }

            var i = 0;
            while (fronts[i].Count > 0)
            {
                var front = new List<T>();
                foreach (var p in fronts[i])
                {
                    foreach (var q in dominationList[p])
                    {
                        dominatedCount[q]--;
                        if (dominatedCount[q] == 0)
                            front.Add(q);
                    }
                }

                i++;
                fronts.Add(i, front);
            }

            fronts.Remove(i);
            return fronts.Select(f => f.Value.ToImmutableArray());
        }
    }
}