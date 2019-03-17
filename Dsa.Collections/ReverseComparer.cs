using System.Collections.Generic;

namespace Dsa.Collections
{
    /// <summary>
    /// Allows to revert the standard comparer
    /// </summary>
    /// <typeparam name="TElement">Type of compared element</typeparam>
    public class ReverseComparer<TElement> : IComparer<TElement>
    {
        private readonly IComparer<TElement> _baseComaprer;

        private ReverseComparer()
            :this(Comparer<TElement>.Default)
        {
        }

        private ReverseComparer(IComparer<TElement> baseComaprer)
        {
            _baseComaprer = baseComaprer;
        }

        public int Compare(TElement x, TElement y)
        {
            return _baseComaprer.Compare(y, x);
        }

        /// <summary>
        /// Returns a default reverser sort order comparer for the type specified by the generic argument.
        /// </summary>
        public static ReverseComparer<TElement> Default => new ReverseComparer<TElement>();
    }
}