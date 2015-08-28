using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuX.Solver.Support
{
    /// <summary>
    /// Extension methods for IEnumerable.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Gets a random item from the list. Returns <c>null</c> if the list is empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list to search in.</param>
        /// <param name="rng">An optional random number generator.</param>
        /// <returns></returns>
        public static T GetRandom<T>(this IEnumerable<T> list, Random rng = null) where T : class
        {
            rng = rng ?? new Random();

            var thelist = list.ToList();
            if (thelist.Count == 0)
                return null;

            return thelist[rng.Next(thelist.Count)];
        }

        /// <summary>
        /// Gets all unique pairs that can be made from the supplied list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException">List must be 2 or longer to get pairs.</exception>
        public static IEnumerable<Tuple<T, T>> GetAllPairs<T>(this IEnumerable<T> list)
        {
            if (list == null) throw new ArgumentNullException();

            var fulllist = list.ToArray();
            if (fulllist.Length < 2) throw new InvalidOperationException("List must be 2 or longer to get pairs.");

            for (int i1 = 0; i1 < fulllist.Length - 1; i1++)
            {
                for (int i2 = i1 + 1; i2 < fulllist.Length; i2++)
                {
                    yield return Tuple.Create(fulllist[i1], fulllist[i2]);
                }
            }
        }

        /// <summary>
        /// Gets all unique triplets that can be made from the supplied list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException">List must be 2 or longer to get pairs.</exception>
        public static IEnumerable<Tuple<T, T, T>> GetAllTriplets<T>(this IEnumerable<T> list)
        {
            if (list == null) throw new ArgumentNullException();

            var fulllist = list.ToArray();
            if (fulllist.Length < 3) throw new InvalidOperationException("List must be 3 or longer to get triplets.");

            for (int i1 = 0; i1 < fulllist.Length - 2; i1++)
            {
                for (int i2 = i1 + 1; i2 < fulllist.Length - 1; i2++)
                {
                    for (int i3 = i2 + 1; i3 < fulllist.Length; i3++)
                    {
                        yield return Tuple.Create(fulllist[i1], fulllist[i2], fulllist[i3]);
                    }
                }
            }
        }

    }
}
