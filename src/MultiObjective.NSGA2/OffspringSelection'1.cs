using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.MultiObjective.Abstractions;
using Bunnypro.GeneticAlgorithm.MultiObjective.Primitives;
using Bunnypro.GeneticAlgorithm.Primitives;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.NSGA2
{
    public class OffspringSelection<T> : IDistinctMultiObjectiveGeneticOperation<T> where T : Enum
    {
        private readonly IParetoSort<T> _sorter;
        private readonly IReadOnlyDictionary<T, double> _coefficients;

        public OffspringSelection(IReadOnlyDictionary<T, double> coefficients)
        {
            _sorter = new FastNonDominatedSort<T>(coefficients);
            _coefficients = coefficients;
        }

        public Task<ImmutableHashSet<IChromosome<T>>> Operate(IEnumerable<IChromosome<T>> parents,
            PopulationCapacity capacity, CancellationToken token = default)
        {
            var elite = ImmutableHashSet.CreateBuilder<IChromosome<T>>();
            ImmutableArray<IChromosome<T>> lastFront;
            var fronts = _sorter.Sort(parents).Select(f => f.ToArray()).ToArray();
            foreach (var front in fronts)
            {
                if (elite.Count + front.Length > capacity.Maximum)
                {
                    lastFront = front.ToImmutableArray();
                    break;
                }
                elite.UnionWith(front);
                if (elite.Count > capacity.Minimum) break;
            }

            if (elite.Count >= capacity.Minimum) return Task.FromResult(elite.ToImmutable());

            token.ThrowIfCancellationRequested();

            var remains = capacity.Minimum - elite.Count;
            var crowdingDistance = lastFront.ToDictionary(c => c, c => 0d);
            var normalizedComparable = Normalize(lastFront.Union(elite));

            foreach (var objective in Enum.GetValues(typeof(T)).Cast<T>())
            {
                var sorted = normalizedComparable.OrderBy(c => c.Value[objective]).ToArray();
                if (crowdingDistance.ContainsKey(sorted[0].Key)) crowdingDistance[sorted[0].Key] += 1;
                if (crowdingDistance.ContainsKey(sorted[sorted.Length - 1].Key)) crowdingDistance[sorted[sorted.Length - 1].Key] += 1;
                for (var i = 1; i < sorted.Length - 1; i++)
                {
                    if (!crowdingDistance.ContainsKey(sorted[i].Key)) continue;
                    var lowerValue = sorted[i - 1].Value[objective];
                    var upperValue = sorted[i + 1].Value[objective];
                    var distance = Math.Abs(upperValue - lowerValue);
                    crowdingDistance[sorted[i].Key] += distance;
                }
            }

            elite.UnionWith(crowdingDistance.OrderByDescending(d => d.Value).Select(d => d.Key).Take(remains));
            return Task.FromResult(elite.ToImmutable());
        }

        private IEnumerable<KeyValuePair<IChromosome<T>, ObjectiveValues<T>>> Normalize(IEnumerable<IChromosome<T>> chromosomes)
        {
            var comparable = chromosomes.ToArray();
            var objectives = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            var normalizers = objectives
                .ToDictionary<T, T, Func<double, double>>(o => o, objective =>
                {
                    var sign = Math.Sign(_coefficients[objective]);
                    var ordered = comparable.OrderByDescending(c => c.ObjectiveValues[objective] * sign).ToArray();
                    var minimumValue = ordered.Last().ObjectiveValues[objective];
                    var maximumValue = ordered.First().ObjectiveValues[objective];
                    var rangeValue = Math.Abs(maximumValue - minimumValue);
                    return value => rangeValue <= 0 ? 1 : (Math.Abs(value - minimumValue) / rangeValue);
                });

            return comparable.Select(c =>
            {
                var values = objectives.ToDictionary(o => o,
                    objective => normalizers[objective].Invoke(c.ObjectiveValues[objective]));
                return new KeyValuePair<IChromosome<T>, ObjectiveValues<T>>(c, new ObjectiveValues<T>(values));
            });
        }
    }
}