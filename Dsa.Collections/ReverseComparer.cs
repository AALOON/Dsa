using System.Collections.Generic;

namespace Dsa.Collections
{
    public class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> _baseComaprer;

        private ReverseComparer()
            :this(Comparer<T>.Default)
        {
        }

        private ReverseComparer(IComparer<T> baseComaprer)
        {
            _baseComaprer = baseComaprer;
        }

        public int Compare(T x, T y)
        {
            return _baseComaprer.Compare(y, x);
        }

        /// <summary>
        /// Returns a default reverser sort order comparer for the type specified by the generic argument.
        /// </summary>
        public static ReverseComparer<T> Default => new ReverseComparer<T>();
    }
}