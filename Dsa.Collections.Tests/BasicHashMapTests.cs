using System;
using FluentAssertions;
using Xunit;

namespace Dsa.Collections.Tests
{
    public class BasicHashMapTests
    {
        [Fact]
        public void HashMap_IsEmpty()
        {
            var map = new HashMap<int, int>();
            map.Count.Should().Be(0);
        }

        [Fact]
        public void HashMapAddGet_Success()
        {
            const int key = 10;
            const int value = 10;
            var map = new HashMap<int, int>();
            map.Add(key, value);
            var val = map[key];
            value.Should().Be(val);
            map.Count.Should().Be(1);
        }

        [Fact]
        public void HashMapGetSet_Success()
        {
            const int key = 10;
            const int value = 10;
            var map = new HashMap<int, int>();
            map[key] = value;
            var val = map[key];
            value.Should().Be(val);
        }

        [Fact]
        public void HashMapGetSet_Update_Success()
        {
            const int key = 10;
            const int value = 10;
            const int updatedValue = 50;
            var map = new HashMap<int, int>();
            map[key] = value;
            map[key] = updatedValue;
            var val = map[key];
            updatedValue.Should().Be(val);
        }

        [Fact]
        public void HashMapAddGet_Sequnce_Success()
        {
            const int count = 1000;
            var map = new HashMap<int, int>();
            for (int i = 0; i < count; i++)
                map.Add(i, i + 10000);

            map.Count.Should().Be(count);

            for (int i = 0; i < count; i++)
                map[i].Should().Be(i + 10000);
        }

        [Fact]
        public void HashMapAddGet_RandomSequnce_Success()
        {
            const int count = 1000;
            var totalCount = 0;
            var map = new HashMap<int, int>();
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                var next = random.Next();
                if(map.ContainsKey(next))
                    continue;
                map.Add(next, next + 10000);
                totalCount++;
            }

            map.Count.Should().Be(totalCount);

            foreach (var key in map.Keys)
                map[key].Should().Be(key + 10000);
        }

        [Fact]
        public void HashMapAddRemove_Sequnce_Success()
        {
            const int count = 1000;
            var map = new HashMap<int, int>();
            for (int i = 0; i < count; i++)
                map.Add(i, i + 10000);

            map.Count.Should().Be(count);

            for (int i = 0; i < count; i++)
                map.Remove(i).Should().BeTrue();

            map.Count.Should().Be(0);
        }

        [Fact]
        public void HashMapAddRemove_Sequnce_Unsuccess()
        {
            const int count = 1000;
            var map = new HashMap<int, int>();
            for (int i = 0; i < count; i++)
                map.Add(i, i + 10000);

            map.Count.Should().Be(count);

            for (int i = count; i < 2000; i++)
                map.Remove(i).Should().BeFalse();

            map.Count.Should().Be(count);
        }

        [Fact]
        public void HashMapContainsKey_Sequnce_Success()
        {
            const int count = 1000;
            var map = new HashMap<int, int>();
            for (int i = 0; i < count; i++)
                map.Add(i, i + 10000);

            map.Count.Should().Be(count);

            for (int i = 0; i < count; i++)
                map.ContainsKey(i).Should().BeTrue();
        }

        [Fact]
        public void HashMapContainsKey_Sequnce_Unsuccess()
        {
            const int count = 1000;
            var map = new HashMap<int, int>();
            for (int i = 0; i < count; i++)
                map.Add(i, i + 10000);

            map.Count.Should().Be(count);

            for (int i = count; i < 2000; i++)
                map.ContainsKey(i).Should().BeFalse();
        }
    }
}