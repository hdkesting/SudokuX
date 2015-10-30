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

            foreach (var pairIndices in GetSubsets(0, fulllist.Length - 1, 2))
            {
                yield return Tuple.Create(fulllist[pairIndices[0]], fulllist[pairIndices[1]]);
            }
        }

        /// <summary>
        /// Gets all unique triplets that can be made from the supplied list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException">List must be 3 or longer to get triplets.</exception>
        public static IEnumerable<Tuple<T, T, T>> GetAllTriplets<T>(this IEnumerable<T> list)
        {
            if (list == null) throw new ArgumentNullException();

            var fulllist = list.ToArray();
            if (fulllist.Length < 3) throw new InvalidOperationException("List must be 3 or longer to get triplets.");

            foreach(var tripletIndices in GetSubsets(0, fulllist.Length - 1, 3))
            {
                yield return Tuple.Create(fulllist[tripletIndices[0]], fulllist[tripletIndices[1]], fulllist[tripletIndices[2]]);
            }
        }

        /// <summary>
        /// Gets all unique quads (sets of 4) that can be made from the supplied list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException">List must be 4 or longer to get quads.</exception>
        public static IEnumerable<Tuple<T,T,T,T>> GetAllQuads<T>(this IEnumerable<T> list)
        {
            if (list == null) throw new ArgumentNullException();

            var fulllist = list.ToArray();
            if (fulllist.Length < 4) throw new InvalidOperationException("List must be 4 or longer to get quads.");

            foreach (var indices in GetSubsets(0, fulllist.Length - 1, 4))
            {
                yield return Tuple.Create(fulllist[indices[0]], fulllist[indices[1]], fulllist[indices[2]], fulllist[indices[3]]);
            }
        }

        /// <summary>
        /// Gets the unique subsets of a specified length from a range of numbers.
        /// </summary>
        /// <param name="min">The inclusive minimum.</param>
        /// <param name="max">The inclusive maximum.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static IEnumerable<List<int>> GetSubsets(int min, int max, int count)
        {
            if (count <= 0) yield break;

            if (count == 1)
            {
                for (int i = min; i <= max; i++)
                {
                    yield return new List<int> { { i } };
                }
            }
            else
            {
                for (int i = min; i <= max - count + 1; i++)
                {
                    foreach (var combo in GetSubsets(i + 1, max, count - 1))
                    {
                        var res = new List<int>(combo);
                        res.Insert(0, i);
                        yield return res;
                    }
                }
            }
        }
    }
}
