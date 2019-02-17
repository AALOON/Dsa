using System.Collections.Generic;

namespace Dsa.Collections.Extensions
{
    /// <summary>
    /// Help function for comparer
    /// </summary>
    public static class ComparerExtensions
    {
        public static bool IsLess<TElement>(
            this TElement a, TElement b, IComparer<TElement> comparer)
        {
            return comparer.Compare(a, b) < 0;
        }

        public static bool IsMore<TElement>(
            this TElement a, TElement b, IComparer<TElement> comparer)
        {
            return comparer.Compare(a, b) > 0;
        }

        public static bool IsEqual<TElement>(
            this TElement a, TElement b, IComparer<TElement> comparer)
        {
            return comparer.Compare(a, b) == 0;
        }
    }
}