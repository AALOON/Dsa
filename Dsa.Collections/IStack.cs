namespace Dsa.Collections
{
    /// <summary>
    /// Interface for stack data structure
    /// LIFO
    /// </summary>
    /// <typeparam name="TElement">Type of stored element</typeparam>
    public interface IStack<TElement>
    {
        /// <summary>
        /// Adds new element to the stack
        /// </summary>
        /// <param name="element"></param>
        void Push(TElement element);

        /// <summary>
        /// Clears the queue
        /// </summary>
        void Clear();

        /// <summary>
        /// Get the first element in queue but does not remove it
        /// </summary>
        TElement Peek();

        /// <summary>
        /// Gets the first available element of queue and removes it 
        /// </summary>
        TElement Pop();
    }
}