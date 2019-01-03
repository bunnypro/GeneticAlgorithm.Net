using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bunnypro.GeneticAlgorithm.Core.Chromosomes;
using Bunnypro.GeneticAlgorithm.Standard;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Fitness
{
    public sealed class MultiObjectiveFitnessEvaluator<T> : IMultiObjectiveFitnessEvaluator<T>, IReadOnlyDictionary<T, IFitnessEvaluator> where T : Enum
    {
        private readonly IDictionary<T, IFitnessEvaluator> _evaluators = new Dictionary<T, IFitnessEvaluator>();

        public int Count => _evaluators.Count;
        public IEnumerable<T> Keys => _evaluators.Keys;
        public IEnumerable<IFitnessEvaluator> Values => _evaluators.Values;

        public IFitnessEvaluator this[T key] => _evaluators[key];

        public void Add(T objective, IFitnessEvaluator evaluator)
        {
            _evaluators.Add(objective, evaluator);
        }

        public IComparable Evaluate(IChromosome chromosome)
        {
            var value = new ObjectiveValues<T>();
            EvaluateTo(value, chromosome);
            return value;
        }

        public void EvaluateTo(IObjectiveValues<T> value, IChromosome chromosome)
        {
            foreach (var objective in Enum.GetValues(typeof(T)).Cast<T>())
                if (_evaluators.ContainsKey(objective))
                    value[objective] = _evaluators[objective].Evaluate(chromosome);
        }

        public IEnumerator<KeyValuePair<T, IFitnessEvaluator>> GetEnumerator()
        {
            return _evaluators.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(T key)
        {
            return _evaluators.ContainsKey(key);
        }

        public bool TryGetValue(T key, out IFitnessEvaluator value)
        {
            return _evaluators.TryGetValue(key, out value);
        }
    }
}