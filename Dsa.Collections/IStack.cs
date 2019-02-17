namespace Dsa.Collections
{
    public interface IStack<TElement>
    {
        void Push(TElement element);
        void Clear();
        TElement Peek();
        TElement Pop();
    }
}