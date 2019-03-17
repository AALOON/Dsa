using System;
using System.Collections;
using System.Collections.Generic;

namespace Dsa.Collections
{
    /// <summary>
    /// Prefix tree (Trie)
    /// </summary>
    public class Trie<TSequence, TChar> : ICollection<TSequence> where TSequence : IEnumerable<TChar>
    {
        private readonly Func<IEnumerable<TChar>, TSequence> _sequenceFactory;

        private class Node : IEnumerable<Node>, IEnumerable<TChar>
        {
            private const int ChildsCapacity = 1;
            private readonly IDictionary<TChar, Node> _childs;

            public Node(TChar value, IEqualityComparer<TChar> comparer)
            {
                Value = value;
                _childs = new Dictionary<TChar, Node>(ChildsCapacity, comparer);
            }

            public Node(Node parent, TChar value, IEqualityComparer<TChar> comparer)
            {
                Value = value;
                _childs = new Dictionary<TChar, Node>(ChildsCapacity, comparer);
                Parent = parent;
            }

            public TChar Value { get; }

            public Node Parent { get; }

            public bool IsSequenceEnd { get; set; }

            public bool Contains(TChar item)
            {
                return _childs.ContainsKey(item);
            }

            public bool Any()
            {
                return _childs.Count > 0;
            }

            public Node this[TChar key]
            {
                get => _childs[key];
                set => _childs[key] = value;
            }

            public void Remove(TChar key)
            {
                _childs.Remove(key);
            }

            public static Node GetRoot(IEqualityComparer<TChar> comparer)
            {
                return new Node(default(TChar), comparer);
            }

            IEnumerator<TChar> IEnumerable<TChar>.GetEnumerator()
            {
                var current = this;

                var stak = new Stack<TChar>();

                while (current.Parent != null)
                {
                    stak.Push(current.Value);
                    current = current.Parent;
                }

                return stak.GetEnumerator();
            }

            public IEnumerator<Node> GetEnumerator()
            {
                return _childs.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private readonly IEqualityComparer<TChar> _comparer;

        private Node _root;

        public Trie(Func<IEnumerable<TChar>, TSequence> sequenceFactory)
        {
            _sequenceFactory = sequenceFactory;
            _comparer = EqualityComparer<TChar>.Default;
            Clear();
        }

        public Trie(Func<IEnumerable<TChar>, TSequence> sequenceFactory, IEqualityComparer<TChar> comparer)
        {
            _sequenceFactory = sequenceFactory;
            _comparer = comparer;
            Clear();
        }

        public IEnumerator<TSequence> GetEnumerator()
        {
            return GetEnumeratorInternal(_root);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TSequence sequence)
        {
            var current = _root;
            foreach (var item in sequence)
            {
                if (!current.Contains(item))
                    current[item] = new Node(current, item, _comparer);
                current = current[item];
            }

            if (!current.IsSequenceEnd)
            {
                current.IsSequenceEnd = true;
                Count++;
            }
        }

        public void Clear()
        {
            _root = Node.GetRoot(_comparer);
            Count = 0;
        }

        public bool Contains(TSequence sequence)
        {
            var current = _root;
            foreach (var item in sequence)
            {
                if (!current.Contains(item))
                    return false;
                current = current[item];
            }

            return current.IsSequenceEnd;
        }

        public void CopyTo(TSequence[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex),
                    $"Wrong index [{arrayIndex}] need to be [{0}, {array.Length - 1}]!");
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("Wrong values of array and/or arrayIndex");

            foreach (var str in this)
            {
                array[arrayIndex] = str;
                arrayIndex++;
            }
        }

        public bool Remove(TSequence item)
        {
            var node = FindInternal(item);
            if (node == null)
                return false;

            node.IsSequenceEnd = false;
            Count--;

            var current = node;
            while (current != null && !current.Any() && !current.IsSequenceEnd)
            {
                current.Parent?.Remove(current.Value);
                current = current.Parent;
            }
            return true;
        }

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        private Node FindInternal(TSequence sequence)
        {
            var current = _root;
            foreach (var item in sequence)
            {
                if (!current.Contains(item))
                    return null;
                current = current[item];
            }

            if (current.IsSequenceEnd)
                return current;
            return null;
        }

        private IEnumerator<TSequence> GetEnumeratorInternal(Node current)
        {
            foreach (var node in current)
            {
                if (node.IsSequenceEnd)
                    yield return _sequenceFactory(node);

                using (var nodeEnumerator = GetEnumeratorInternal(node))
                    if (nodeEnumerator.MoveNext())
                        yield return nodeEnumerator.Current;
            }
        }
    }
}