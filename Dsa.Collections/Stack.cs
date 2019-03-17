using System.Collections;
using System.Collections.Generic;

namespace Dsa.Collections
{
    /// <summary>
    /// Stack implementation.
    /// </summary>
    /// <typeparam name="TElement">Type of the stored element</typeparam>
    public class Stack<TElement> : IStack<TElement>, IEnumerable<TElement>
    {
        private readonly SingleLinkedList<TElement> _linkedList;

        public Stack()
        {
            _linkedList = new SingleLinkedList<TElement>();
        }

        /// <inheritdoc />
        public TElement Peek()
        {
            return _linkedList.First();
        }

        /// <inheritdoc />
        public void Push(TElement element)
        {
            _linkedList.InsertFirst(element);
        }

        /// <inheritdoc />
        public TElement Pop()
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