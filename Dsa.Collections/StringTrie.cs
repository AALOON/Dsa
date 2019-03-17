using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Dsa.Collections
{
    /// <summary>
    /// Prefix string tree (Trie)
    /// </summary>
    public class StringTrie : ICollection<string>
    {
        private class Node : IEnumerable<Node>
        {
            private const int ChildsCapacity = 1;
            private readonly IDictionary<char, Node> _childs;

            public Node(char value, IEqualityComparer<char> comparer)
            {
                Value = value;
                _childs = new Dictionary<char, Node>(ChildsCapacity, comparer);
            }

            public Node(Node parent, char value, IEqualityComparer<char> comparer)
            {
                Value = value;
                _childs = new Dictionary<char, Node>(ChildsCapacity, comparer);
                Parent = parent;
            }

            public char Value { get; }

            public Node Parent { get; }

            public bool IsSequenceEnd { get; set; }

            public bool Contains(char item)
            {
                return _childs.ContainsKey(item);
            }

            public bool Any()
            {
                return _childs.Count > 0;
            }

            public Node this[char key]
            {
                get => _childs[key];
                set => _childs[key] = value;
            }

            public void Remove(char key)
            {
                _childs.Remove(key);
            }

            public static Node GetRoot(IEqualityComparer<char> comparer)
            {
                return new Node(default(char), comparer);
            }

            public IEnumerator<Node> GetEnumerator()
            {
                return _childs.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                var current = this;

                while (current.Parent != null)
                {
                    sb.Insert(0, current.Value);
                    current = current.Parent;
                }

                return sb.ToString();
            }
        }

        private readonly IEqualityComparer<char> _comparer;

        private Node _root;

        public StringTrie()
        {
            _comparer = EqualityComparer<char>.Default;
            Clear();
        }


        public IEnumerator<string> GetEnumerator()
        {
            return GetEnumeratorInternal(_root);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string sequence)
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

        public bool Contains(string sequence)
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

        public void CopyTo(string[] array, int arrayIndex)
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

        public bool Remove(string item)
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

        private Node FindInternal(string sequence)
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

        private IEnumerator<string> GetEnumeratorInternal(Node current)
        {
            foreach (var node in current)
            {
                if (node.IsSequenceEnd)
                    yield return node.ToString();

                using (var nodeEnumerator = GetEnumeratorInternal(node))
                    if (nodeEnumerator.MoveNext())
                        yield return nodeEnumerator.Current;
            }
        }
    }
}