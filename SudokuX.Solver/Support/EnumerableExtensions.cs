using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuX.Solver.Support
{
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
    }
}
