using System;
using System.Collections;
using System.Collections.Generic;

namespace Dsa.Collections
{
    /// <summary>
    /// Single linked list implementation.
    /// Add             - O(1);
    /// InsertFirst     - O(1);
    /// Clear           - O(1);
    /// Contains        - O(n);
    /// CopyTo          - O(n);
    /// Remove          - O(n);
    /// RemoveFirst     - O(1);
    /// Count           - O(1);
    /// </summary>
    /// <typeparam name="TElement">Type of the stored element</typeparam>
    public class SingleLinkedList<TElement> : ICollection<TElement>
    {
        private class Node
        {
            public Node(TElement value)
            {
                Value = value;
            }

            public Node Next { get; set; }

            public TElement Value { get; }
        }

        private readonly IEqualityComparer<TElement> _comparer;

        private Node _head;
        private Node _tail;
        private int _count;

        public SingleLinkedList()
        {
            _count = 0;
            _comparer = EqualityComparer<TElement>.Default;
        }

        public SingleLinkedList(IEqualityComparer<TElement> comparer)
        {
            _count = 0;
            _comparer = comparer;
        }

        /// <inheritdoc />
        public int Count => _count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public void Add(TElement item)
        {
            _count++;
            if (_head == null)
            {
                _head = new Node(item);
                _tail = _head;
                return;
            }
            var tmp = _tail;
            tmp.Next = new Node(item);
            _tail = tmp.Next;
        }

        public void InsertFirst(TElement item)
        {
            _count++;
            if (_head == null)
            {
                _head = new Node(item);
                return;
            }
            var tmp = _head;
            _head = new Node(item)
            {
                Next = tmp
            };
        }

        /// <inheritdoc />
        public void Clear()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }

        /// <inheritdoc />
        public bool Contains(TElement item)
        {
            var current = _head;
            while (current != null && !_comparer.Equals(current.Value, item))
            {
                current = current.Next;
            }
            if (current == null)
                return false;
            return true;
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
            var current = _head;
            while (current != null)
            {
                array[arrayIndex] = current.Value;
                current = current.Next;
                arrayIndex++;
            }
        }

        /// <inheritdoc />
        public bool Remove(TElement item)
        {
            var prev = _head;
            var current = _head;
            while (current != null && !_comparer.Equals(current.Value, item))
            {
                prev = current;
                current = prev.Next;
            }
            if (current == null)
                return false;
            prev.Next = current.Next;
            if (_head == current)
                _head = current.Next;
            _count--;
            return true;
        }

        public TElement RemoveFirst()
        {
            if (_head == null)
                throw new ArgumentNullException();
            var removedNode = _head;
            _head = removedNode.Next;
            if (_head == null)
                _tail = null;
            return removedNode.Value;
        }

        public TElement First()
        {
            if (_head == null)
                throw new ArgumentNullException();
            return _head.Value;
        }

        public TElement Last()
        {
            if (_tail == null)
                throw new ArgumentNullException();
            return _tail.Value;
        }

        /// <inheritdoc />
        public IEnumerator<TElement> GetEnumerator()
        {
            var current = _head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}