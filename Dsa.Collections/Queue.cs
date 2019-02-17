using System.Collections;
using System.Collections.Generic;

namespace Dsa.Collections
{
    public class Queue<TElement> : IQueue<TElement>, IEnumerable<TElement>
    {
        private readonly SingleLinkedList<TElement> _linkedList;

        public Queue()
        {
            _linkedList = new SingleLinkedList<TElement>();
        }

        public TElement Peek()
        {
            return _linkedList.First();
        }

        public void Enqueue(TElement element)
        {
            _linkedList.Add(element);
        }

        public TElement Dequeue()
        {
            return _linkedList.RemoveFirst();
        }

        public void Clear()
        {
            _linkedList.Clear();
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return _linkedList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}