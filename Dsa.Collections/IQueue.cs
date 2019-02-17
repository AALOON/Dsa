namespace Dsa.Collections
{
    public interface IQueue<TElement>
    {
        TElement Peek();
        void Clear();
        TElement Dequeue();
        void Enqueue(TElement element);
    }
}