using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bunnypro.GeneticAlgorithm.MultiObjective.Abstractions;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Core
{
    public abstract class NormalizedObjectiveValuesFitnessEvaluator<T> : IChromosomeEvaluator<T> where T : Enum
    {
        private readonly IReadOnlyDictionary<T, double> _coefficients;

        public NormalizedObjectiveValuesFitnessEvaluator() :
            this(Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(k => k, _ => 1d))
        {
        }

        public NormalizedObjectiveValuesFitnessEvaluator(IReadOnlyDictionary<T, double> coefficients)
        {
            _coefficients = coefficients;
        }

        public async Task EvaluateAll(IEnumerable<IChromosome<T>> chromosomes, CancellationToken token = default)
        {
            var chromosomesArray = chromosomes.ToArray();
            await EvaluateObjectiveValuesAll(chromosomesArray, token);
            await EvaluateFitnessValueAll(chromosomesArray);
        }

        private async Task EvaluateFitnessValueAll(IEnumerable<IChromosome<T>> chromosomes)
        {
            var chromosomeArray = chromosomes.ToArray();
            var objectivesNormalizer = Enum.GetValues(typeof(T)).Cast<T>()
                .ToDictionary<T, T, Func<double, double>>(
                    objective => objective,
                    objective =>
                    {
                        var ordered = chromosomeArray
                            .Select(chromosome => chromosome.ObjectiveValues[objective])
                            .OrderBy(value => value * _coefficients[objective])
                            .ToArray();

                        var boundary = new
                        {
                            Max = ordered.Last(),
                            Min = ordered.First()
                        };

                        var range = Math.Abs(boundary.Max - boundary.Min);

                        return value => range <= 0 ? 1 : (Math.Abs(value - boundary.Min) / range);
                    });

            var coefficients = _coefficients.ToDictionary(c => c.Key, c => Math.Abs(c.Value));
            var totalCoefficient = coefficients.Values.Sum();
            var fitnessEvaluationTasks = chromosomeArray.Select(chromosome => Task.Run(() =>
            {
                chromosome.Fitness = chromosome.ObjectiveValues.Sum(
                    objective => objectivesNormalizer[objective.Key].Invoke(objective.Value) * coefficients[objective.Key]
                ) / totalCoefficient;
            }));

            await Task.WhenAll(fitnessEvaluationTasks);
        }

        protected abstract Task EvaluateObjectiveValuesAll(
            IEnumerable<IChromosome<T>> chromosomes,
            CancellationToken token = default);
    }
}