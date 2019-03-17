namespace Dsa.Collections
{
    /// <summary>
    /// Interface for queue data structure
    /// FIFO
    /// </summary>
    /// <typeparam name="TElement">Type of stored element</typeparam>
    public interface IQueue<TElement>
    {
        /// <summary>
        /// Get the first element in queue but does not remove it
        /// </summary>
        TElement Peek();

        /// <summary>
        /// Clears the queue
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the first available element of queue and removes it 
        /// </summary>
        TElement Dequeue();

        /// <summary>
        /// Adds element to the end of the queue
        /// </summary>
        void Enqueue(TElement element);
    }
}