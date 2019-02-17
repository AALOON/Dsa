using System;
using System.Collections;
using System.Collections.Generic;
using Dsa.Collections.Extensions;

namespace Dsa.Collections
{
    /// <summary>
    /// Heap implementation.
    /// Add             - O(log(n));
    /// Clear           - O(1);
    /// Contains        - O(n);
    /// CopyTo          - O(n);
    /// Remove(TElement)- O(n);
    /// Poll            - O(log(n));
    /// Peek            - O(1);
    /// Count           - O(1);
    /// </summary>
    /// <typeparam name="TElement">Type of the stored element</typeparam>
    public class Heap<TElement> : ICollection<TElement>
    {
        private const int DefaultCapacity = 7;
        private const int Zero = 0;

        private TElement[] _items;
        private readonly IComparer<TElement> _comparer;
        private int _capacity;
        private int _count = 0;

        public Heap()
            : this(Comparer<TElement>.Default)
        {
        }

        public Heap(IComparer<TElement> comparer)
        {
            _capacity = DefaultCapacity;
            _items = new TElement[_capacity];
            _comparer = comparer;
        }

        /// <inheritdoc />
        public int Count => _count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public void Add(TElement item)
        {
            EnshureCapacity();
            _items[_count] = item;
            HeapifyUp(_count);
            _count++;
        }

        /// <inheritdoc />
        public void Clear()
        {
            _count = 0;
        }

        /// <inheritdoc />
        public bool Contains(TElement item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_items[i].IsEqual(item, _comparer))
                    return true;
            }
            return false;
        }
        
        /// <inheritdoc />
        public void CopyTo(TElement[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex),
                    $"Wrong index [{arrayIndex}] need to be [{0}, {array.Length - 1}]!");
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("Wrong values of array and/or arrayIndex");
            
            for (int i = 0; i < _count; i++, arrayIndex++)
            {
                array[arrayIndex] = _items[i];
            }
        }

        /// <inheritdoc />
        public bool Remove(TElement item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_items[i].IsEqual(item, _comparer))
                {
                    _items[i] = _items[_count];
                    _items[_count] = default(TElement); // Not necessary

                    if (_count != i)
                    {
                        if (_items[i].IsLess(GetParent(i), _comparer))
                            HeapifyUp(i);
                        else
                            HeapifyDown(i);
                    }

                    _count--;
                    return true;
                }
            }
            return false;
        }

        /// <inheritdoc />
        public IEnumerator<TElement> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _items[i];
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TElement Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Heap is empty!");
            return _items[Zero];
        }

        public TElement Poll()
        {
            if (_count == 0)
                throw new InvalidOperationException("Heap is empty!");

            var item = _items[Zero];
            _items[Zero] = _items[_count - 1];
            _items[_count - 1] = default(TElement); // Not necessary
            _count--;

            HeapifyDown(Zero);
            return item;
        }

        private int GetLeftChildIndex(int index) => 2 * index + 1;
        private int GetRigthChildIndex(int index) => 2 * index + 2;
        private int GetParentIndex(int index) => (index - 1) / 2;

        private bool HasLeftChild(int index) => GetLeftChildIndex(index) < _count;
        private bool HasRigthChild(int index) => GetRigthChildIndex(index) < _count;
        private bool HasParent(int index) => index > Zero;

        private TElement GetLeftChild(int index) => _items[GetLeftChildIndex(index)];
        private TElement GetRigthChild(int index) => _items[GetRigthChildIndex(index)];
        private TElement GetParent(int index) => _items[GetParentIndex(index)];

        private void Swap(int i, int j)
        {
            var tmp = _items[i];
            _items[i] = _items[j];
            _items[j] = tmp;
        }

        private void HeapifyUp(int index)
        {
            while (HasParent(index) && _items[index].IsLess(GetParent(index), _comparer))
            {
                var newindex = GetParentIndex(index);
                Swap(newindex, index);
                index = newindex;
            }
        }

        private void HeapifyDown(int index)
        {
            while (HasLeftChild(index))
            {
                var childIndex = GetLeftChildIndex(index);
                if (HasRigthChild(index) && GetLeftChild(index).IsMore(GetRigthChild(index), _comparer))
                    childIndex = GetRigthChildIndex(index);
                if (_items[index].IsMore(_items[childIndex], _comparer))
                {
                    Swap(index, childIndex);
                    index = childIndex;
                }
                else break;
            }
        }

        private void EnshureCapacity()
        {
            if (_count == _capacity)
            {
                _capacity = _capacity * 2 + 1;
                Array.Resize(ref _items, _capacity);
            }
        }
    }
}