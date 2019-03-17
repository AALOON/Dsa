using System.Collections;
using System.Collections.Generic;

namespace Dsa.Collections
{
    /// <summary>
    /// Simple implementation of the queue
    /// </summary>
    /// <typeparam name="TElement">Type of stored element</typeparam>
    public class Queue<TElement> : IQueue<TElement>, IEnumerable<TElement>
    {
        private readonly SingleLinkedList<TElement> _linkedList;

        public Queue()
        {
            _linkedList = new SingleLinkedList<TElement>();
        }

        /// <inheritdoc />
        public TElement Peek()
        {
            return _linkedList.First();
        }

        /// <inheritdoc />
        public void Enqueue(TElement element)
        {
            _linkedList.Add(element);
        }

        /// <inheritdoc />
        public TElement Dequeue()
        {
            return _linkedList.RemoveFirst();
        }

        /// <inheritdoc />
        public void Clear()
        {
            _linkedList.Clear();
        }

        /// <inheritdoc />
        public IEnumerator<TElement> GetEnumerator()
        {
            return _linkedList.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}