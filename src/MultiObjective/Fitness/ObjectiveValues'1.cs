using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bunnypro.GeneticAlgorithm.MultiObjective.Fitness
{
    public class ObjectiveValues<T> : IObjectiveValues<T>, IReadOnlyDictionary<T, IComparable>, IComparable<ObjectiveValues<T>> where T : Enum
    {
        private readonly IDictionary<T, IComparable> _values = new Dictionary<T, IComparable>();

        public IEnumerable<T> Keys => _values.Keys;
        public IEnumerable<IComparable> Values => _values.Values;
        public int Count => _values.Count;
        
        public IComparable this[T objective]
        {
            get => _values[objective];
            set
            {
                if (_values.ContainsKey(objective))
                {
                    _values[objective] = value;
                    return;
                }

                _values.Add(objective, value);
            }
        }

        public bool ContainsKey(T key)
        {
            return _values.ContainsKey(key);
        }

        public bool TryGetValue(T key, out IComparable value)
        {
            return _values.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<T, IComparable>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }
        
        public int CompareTo(object obj)
        {
            if (!(obj is ObjectiveValues<T> other)) throw new FormatException();
            return ReferenceEquals(this, other) ?
                0 :
                CompareTo(other);
        }

        public int CompareTo(ObjectiveValues<T> other)
        {
            var domination = 0;
            foreach (var objective in Enum.GetValues(typeof(T)).Cast<T>())
            {
                var sign = Math.Sign(this[objective].CompareTo(other[objective]));
                if (domination == sign)
                    continue;
                if (domination != 0)
                    return 0;
                domination = sign;
            }

            return domination;
        }
    }
}