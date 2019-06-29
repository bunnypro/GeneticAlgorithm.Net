using System;
using System.Collections;
using System.Collections.Generic;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Primitives
{
    public struct ObjectiveValues<T> : IReadOnlyDictionary<T, IComparable> where T : Enum
    {
        private readonly IDictionary<T, IComparable> _values;

        public ObjectiveValues(IDictionary<T, IComparable> values)
        {
            _values = values;
        }

        public IComparable this[T key] => _values[key];
        public IEnumerable<T> Keys => _values.Keys;
        public IEnumerable<IComparable> Values => _values.Values;
        public int Count => _values.Count;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<T, IComparable>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }
        
        public bool ContainsKey(T key)
        {
            return _values.ContainsKey(key);
        }

        public bool TryGetValue(T key, out IComparable value)
        {
            return _values.TryGetValue(key, out value);
        }
    }
}