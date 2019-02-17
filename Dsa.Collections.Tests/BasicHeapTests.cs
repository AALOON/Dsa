using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Dsa.Collections.Tests
{
    public class BasicHeapTests
    {
        [Fact]
        public void Heap_IsEmpty()
        {
            var heap = new Heap<int>();
            heap.Count.Should().Be(0);
        }

        [Fact]
        public void HeapAdd_SuccessAddition()
        {
            const int value = 10;
            var heap = new Heap<int>();
            heap.Add(value);

            heap.Count.Should().Be(1);
            heap.Peek().Should().Be(value);
        }

        [Fact]
        public void HeapAdd_SuccessThreeAddition()
        {
            const int value1 = 1;
            const int value2 = 2;
            const int value3 = 3;

            var heap = new Heap<int>();

            heap.Add(value1);
            heap.Add(value2);

            heap.Count.Should().Be(2);
            heap.Peek().Should().Be(value1);

            heap.Add(value3);
            heap.Count.Should().Be(3);
            heap.Peek().Should().Be(value1);
            heap.Count.Should().Be(3);
        }

        [Fact]
        public void HeapAdd_SuccessThreeReverseAddition()
        {
            const int value1 = 3;
            const int value2 = 2;
            const int value3 = 1;

            var heap = new Heap<int>();

            heap.Add(value1);
            heap.Add(value2);

            heap.Count.Should().Be(2);
            heap.Peek().Should().Be(value2);

            heap.Add(value3);
            heap.Count.Should().Be(3);
            heap.Peek().Should().Be(value3);
            heap.Count.Should().Be(3);
        }

        [Fact]
        public void HeapAdd_ReverseComparer_SuccessThreeAddition()
        {
            const int value1 = 1;
            const int value2 = 2;
            const int value3 = 3;

            var heap = new Heap<int>(ReverseComparer<int>.Default);

            heap.Add(value1);

            heap.Add(value2);
            heap.Count.Should().Be(2);
            heap.Peek().Should().Be(value2);

            heap.Add(value3);
            heap.Count.Should().Be(3);
            heap.Peek().Should().Be(value3);
            heap.Count.Should().Be(3);
        }

        [Fact]
        public void HeapAdd_ReverseComparer_SuccessThreeReverseAddition()
        {
            const int value1 = 3;
            const int value2 = 2;
            const int value3 = 1;

            var heap = new Heap<int>(ReverseComparer<int>.Default);

            heap.Add(value1);

            heap.Add(value2);
            heap.Count.Should().Be(2);
            heap.Peek().Should().Be(value1);

            heap.Add(value3);
            heap.Count.Should().Be(3);
            heap.Peek().Should().Be(value1);
            heap.Count.Should().Be(3);
        }

        [Fact]
        public void HeapAdd_CheckSequence_Success()
        {
            var heap = new Heap<int>();
            for (var i = 0; i < 1000; i++)
            {
                heap.Add(i);
                var peek = heap.Peek();
                peek.Should().Be(0);
            }
            heap.Count.Should().Be(1000);
        }

        [Fact]
        public void HeapAdd_CheckSequenceReverse_Success()
        {
            var heap = new Heap<int>();
            for (var i = 1000; i < 0; i--)
            {
                heap.Add(i);
                var peek = heap.Peek();
                peek.Should().Be(i);
            }
        }

        [Fact]
        public void HeapPoll_CheckSequence_Success()
        {
            var heap = new Heap<int>();
            for (var i = 0; i < 1000; i++)
                heap.Add(i);

            for (var i = 0; i < 1000; i++)
            {
                var peek = heap.Peek();
                peek.Should().Be(i);
                var poll = heap.Poll();
                poll.Should().Be(i);
            }
        }

        [Fact]
        public void Heap_CheckRandomSequence_Success()
        {
            var heap = new Heap<int>();
            var random = new Random();
            for (var i = 0; i < 1000; i++)
                heap.Add(random.Next());

            var last = heap.Peek();
            for (var i = 0; i < 1000; i++)
            {
                var peek = heap.Peek();
                peek.Should().BeGreaterOrEqualTo(last);
                var poll = heap.Poll();
                poll.Should().BeGreaterOrEqualTo(last);
                peek.Should().Be(poll);
                last = peek;
            }
        }

        [Fact]
        public void Heap_CheckRandomSequenceUnique_Success()
        {
            var set = new HashSet<int>();
            var heap = new Heap<int>();
            var random = new Random();
            for (var i = 0; i < 10000; i++)
            {
                var next = random.Next();
                if (!set.Contains(next))
                {
                    heap.Add(next);
                    set.Add(next);
                }
            }
            heap.Count.Should().Be(set.Count);
            var last = heap.Peek();
            while (set.Count > 0)
            {
                var peek = heap.Peek();
                peek.Should().BeGreaterOrEqualTo(last);
                var poll = heap.Poll();
                poll.Should().BeGreaterOrEqualTo(last);
                peek.Should().Be(poll);
                last = peek;
                set.Remove(last).Should().BeTrue();
            }
        }

        [Fact]
        public void Heap_CheckSequenceReverse_Success()
        {
            var heap = new Heap<int>();
            for (var i = 10; i < 0; i++)
            {
                heap.Add(i);
                var peek = heap.Peek();
                peek.Should().Be(i);
            }
        }

        [Fact]
        public void Heap_ThreePop_Success()
        {
            const int value1 = 1;
            const int value2 = 2;
            const int value3 = 3;

            var heap = new Heap<int> {value1, value2, value3};

            var peek = heap.Peek();
            peek.Should().Be(value1);
            var pop = heap.Poll();
            pop.Should().Be(value1);

            peek = heap.Peek();
            peek.Should().Be(value2);
            pop = heap.Poll();
            pop.Should().Be(value2);

            peek = heap.Peek();
            peek.Should().Be(value3);
            pop = heap.Poll();
            pop.Should().Be(value3);
        }
    }
}