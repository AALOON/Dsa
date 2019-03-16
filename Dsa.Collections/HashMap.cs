using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dsa.Collections
{
    /// <summary>
    /// HashMap(Dictionary, Associative massive) implementation.
    /// Add(KeyValuePair/TKey,TValue/)      - O(1)*;
    /// Clear()                             - O(1)* - creating new backets array;
    /// Contains(KeyValuePair/TKey,TValue/) - O(1)*;
    /// CopyTo                              - O(n);
    /// Remove(KeyValuePair/TKey,TValue/)   - O(1)*;
    /// Add(TKey,TValue)                    - O(1)*;
    /// ContainsKey(TKey)                   - O(1)*;
    /// Remove(TKey)                        - O(1)*;
    /// TryGetValue(TKey)                   - O(1)*;
    /// this(TKey) set;get;                 - O(1)*;
    /// Keys                                - O(n);
    /// Values                              - O(n);
    /// Count                               - O(1);
    /// * in order if there are a good hash function (minimal collisions)
    /// </summary>
    /// <typeparam name="TKey">Type of stored key</typeparam>
    /// <typeparam name="TValue">Type of stored value</typeparam>
    public class HashMap<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private const double DefaultBacketsCapacityFullness = 0.8;
        private const int DefaultBacketsCapacity = 4;
        private const int DefaultBacketsMultiplier = 2;
        private const int DefaultOneBacketCapacity = 1;

        private readonly double _backetsCapacityFullness;
        private readonly int _backetsMultiplier;
        private readonly int _oneBacketCapacity;

        private readonly IEqualityComparer<TKey> _comparer;

        private int _backetsCapacity;

        private IList<KeyValuePair<TKey, TValue>>[] _backets;

        public HashMap()
            : this(EqualityComparer<TKey>.Default)
        {
        }

        public HashMap(int backetsCapacity)
            : this(EqualityComparer<TKey>.Default, backetsCapacity: backetsCapacity)
        {
        }

        public HashMap(IEqualityComparer<TKey> comparer,
            double backetsCapacityFullness = DefaultBacketsCapacityFullness,
            int backetsCapacity = DefaultBacketsCapacity)
        {
            _comparer = comparer;
            _backetsCapacityFullness = backetsCapacityFullness;
            _backetsMultiplier = DefaultBacketsMultiplier;
            _backetsCapacity = backetsCapacity;
            _oneBacketCapacity = DefaultOneBacketCapacity;
            Count = 0;
            _backets = new IList<KeyValuePair<TKey, TValue>>[_backetsCapacity];
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _backets
                .Where(baket => baket != null)
                .SelectMany(baket => baket).GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            EnshureCapacity();
            InternalAdd(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _backets = new IList<KeyValuePair<TKey, TValue>>[_backets.Length];
            Count = 0;
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (InternalTryGet(item.Key, out var pair))
                if (EqualityComparer<KeyValuePair<TKey, TValue>>.Default.Equals(pair, item))
                    return true;
            return false;
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex),
                    $"Wrong index [{arrayIndex}] need to be [{0}, {array.Length - 1}]!");
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("Wrong values of array and/or arrayIndex");

            foreach (var pair in this)
            {
                array[arrayIndex] = pair;
                arrayIndex++;
            }
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!Contains(item))
                return false;
            return InternalRemove(item.Key);
        }

        /// <inheritdoc />
        public int Count { get; private set; }

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public void Add(TKey key, TValue value)
        {
            EnshureCapacity();
            InternalAdd(new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key)
        {
            return InternalTryGet(key, out _);
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            return InternalRemove(key);
        }

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            if (InternalTryGet(key, out var pair))
            {
                value = pair.Value;
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public TValue this[TKey key]
        {
            get => InternalGet(key).Value;
            set => InternalAddOrUpdate(new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys
        {
            get
            {
                return _backets
                    .Where(baket => baket != null)
                    .SelectMany(baket => baket)
                    .Select(pair => pair.Key)
                    .ToList();
            }
        }

        /// <inheritdoc />
        public ICollection<TValue> Values
        {
            get
            {
                return _backets
                    .Where(baket => baket != null)
                    .SelectMany(baket => baket)
                    .Select(pair => pair.Value)
                    .ToList();
            }
        }

        private void EnshureCapacity()
        {
            if (Count > _backetsCapacity * _backetsCapacityFullness)
            {
                _backetsCapacity = _backetsCapacity * _backetsMultiplier;
                var oldBakets = _backets;
                _backets = new IList<KeyValuePair<TKey, TValue>>[_backetsCapacity];
                Count = 0;
                foreach (var baket in oldBakets.Where(baket => baket != null))
                    foreach (var pair in baket)
                        InternalAdd(pair);
            }
        }

        private void InternalAdd(KeyValuePair<TKey, TValue> newPair)
        {
            var backetIndex = GetIndex(newPair.Key);
            var backet = _backets[backetIndex];

            if (backet == null)
                backet = _backets[backetIndex] = new List<KeyValuePair<TKey, TValue>>(_oneBacketCapacity);
            else
                foreach (var pair in backet)
                    if (_comparer.Equals(pair.Key, newPair.Key))
                        throw new ArgumentException(
                            $"Duplicate key. The key: [{newPair.Key} is already exists]", nameof(newPair.Key));

            backet.Add(newPair);
            Count++;
        }

        private bool InternalRemove(TKey key)
        {
            var backetIndex = GetIndex(key);
            var backet = _backets[backetIndex];

            if (backet == null)
                return false;
            for (var i = 0; i < backet.Count; i++)
                if (_comparer.Equals(backet[i].Key, key))
                {
                    backet.RemoveAt(i);
                    Count--;
                    return true;
                }

            return false;
        }

        private void InternalAddOrUpdate(KeyValuePair<TKey, TValue> newPair)
        {
            var backetIndex = GetIndex(newPair.Key);
            var backet = _backets[backetIndex];

            if (backet == null)
                backet = _backets[backetIndex] = new List<KeyValuePair<TKey, TValue>>(_oneBacketCapacity);
            else
                for (var i = 0; i < backet.Count; i++)
                    if (_comparer.Equals(backet[i].Key, newPair.Key))
                    {
                        backet[i] = newPair;
                        return;
                    }

            backet.Add(newPair);
            Count++;
        }

        private KeyValuePair<TKey, TValue> InternalGet(TKey key)
        {
            if (InternalTryGet(key, out var pair))
                return pair;
            throw new ArgumentNullException(nameof(key), "Value not found for Key");
        }

        private bool InternalTryGet(TKey key, out KeyValuePair<TKey, TValue> pair)
        {
            pair = default(KeyValuePair<TKey, TValue>);
            var backet = InternalGetBacket(key);
            if (backet == null)
                return false;
            foreach (var p in backet)
                if (_comparer.Equals(p.Key, key))
                {
                    pair = p;
                    return true;
                }
            return false;
        }

        private IList<KeyValuePair<TKey, TValue>> InternalGetBacket(TKey key)
        {
            var backetIndex = GetIndex(key);
            var backet = _backets[backetIndex];
            return backet;
        }

        private int GetIndex(TKey key)
        {
            return key.GetHashCode() % _backetsCapacity;
        }
    }
}