using System;
using System.Collections;
using System.Collections.Generic;

namespace Dsa.Collections
{
    public class Trie<TSequence, TChar> : ICollection<TSequence> where TSequence : IEnumerable<TChar>
    {
        private class Node : IEnumerable<Node>
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

            public bool Remove(TChar key)
            {
                return _childs.Remove(key);
            }

            public static Node GetRoot(IEqualityComparer<TChar> comparer)
            {
                return new Node(default(TChar), comparer);
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

        public Trie()
        {
            _comparer = EqualityComparer<TChar>.Default;
            Clear();
        }


        public IEnumerator<TSequence> GetEnumerator()
        {
            throw new NotImplementedException();
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
                if (!current.Contains(item))
                    return false;
            return current.IsSequenceEnd;
        }

        public void CopyTo(TSequence[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TSequence item)
        {
            var node = FindInternal(item);
            if (node == null)
                return false;

            node.IsSequenceEnd = false;
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
                if (!current.Contains(item))
                    return null;
            if (current.IsSequenceEnd)
                return current;
            return null;
        }
    }
}