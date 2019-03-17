using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Dsa.Collections.Tests
{
    public class BasicStringTrieTestsBase : BasicTrieTestsBase
    {
        protected override ICollection<string> CreateTrie()
        {
            return new StringTrie();
        }
    }

    public class BasicGenericTrieTestsBase : BasicTrieTestsBase
    {
        protected override ICollection<string> CreateTrie()
        {
            return new Trie<string, char>(chars => new string(chars.ToArray()));
        }
    }

    public abstract class BasicTrieTestsBase
    {
        private readonly string[] _words = {
            "one", "two", "three", "four", "five",
            "six", "seven", "eight", "nine", "ten"
        };

        [Fact]
        public void StringTrie_IsEmpty()
        {
            var trie = CreateTrie();
            trie.Count.Should().Be(0);
        }

        [Fact]
        public void StringTrie_AddString_Success()
        {
            var trie = CreateTrie();

            const string word = "Hello";
            
            trie.Add(word);

            trie.Count.Should().Be(1);
            trie.Contains(word).Should().BeTrue();
            trie.Single().Should().Be(word);
        }

        [Fact]
        public void StringTrie_RemoveString_Success()
        {
            var trie = CreateTrie();

            const string word = "Hello";
            trie.Add(word);
            trie.Remove(word);

            trie.Count.Should().Be(0);
        }

        [Fact]
        public void StringTrie_AddTenWords_Success()
        {
            var trie = CreateTrie();

            foreach (var word in _words)
                trie.Add(word);


            trie.Count.Should().Be(10);
            foreach (var word in _words)
                trie.Contains(word).Should().BeTrue();
        }

        [Fact]
        public void StringTrie_Clear_Success()
        {
            var trie = CreateTrie();
            foreach (var word in _words)
                trie.Add(word);

            trie.Clear();
            trie.Count.Should().Be(0);
        }

        [Fact]
        public void StringTrie_EnumerableToList_SameSequances()
        {
            var trie = CreateTrie();
            foreach (var word in _words)
                trie.Add(word);

            var list = trie.ToList();
            list.Count.Should().Be(trie.Count);
            list.Should().BeEquivalentTo(trie);
        }

        [Fact]
        public void StringTrie_EnumerableDoNotContainNull_Success()
        {
            var trie = CreateTrie();
            foreach (var word in _words)
                trie.Add(word);

            trie.Where(s => s == null).Should().BeEmpty();
        }

        protected abstract ICollection<string> CreateTrie();
    }
}