using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.SolverStrategies
{
    /// <summary>
    /// X-Wing pattern: one digit in two different rows (or columns) at exactly two positions in the same columns (or rows).
    /// Then the other cells in that column (or row) can't have this digit.
    /// </summary>
    public class XWing : ISolverStrategy
    {
        /// <summary>
        /// Gets the complexity-score of this solver (8).
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        public float Complexity
        {
            get { return 8f; }
        }

        /// <summary>
        /// Processes the grid and returns any helpful conclusions.
        /// </summary>
        /// <param name="grid">The grid to process.</param>
        /// <returns></returns>
        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            for (int digit = grid.MinValue; digit <= grid.MaxValue; digit++)
            {
                var result = FindXWing(digit, grid, GroupType.Row, GroupType.Column);
                if (result.Any())
                    return result;

                // would this work?
                //result = FindXWing(digit, grid, GroupType.SpecialLine, GroupType.Row);
                //if (result.Any())
                //    return result;
                //result = FindXWing(digit, grid, GroupType.SpecialLine, GroupType.Column);
                //if (result.Any())
                //    return result;

                result = FindXWing(digit, grid, GroupType.Column, GroupType.Row);
                if (result.Any())
                    return result;
            }

            return Enumerable.Empty<Conclusion>();
        }

        private IList<Conclusion> FindXWing(int digit, ISudokuGrid grid, GroupType first, GroupType second)
        {
            // find groups of the first type containing exactly two cells with this digit as available
            var groups = grid.CellGroups
                                .Where(g => g.GroupType == first) // just the first type
                                .Where(g => g.Cells.Count(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Contains(digit)) == 2)
                                .ToList();
            if (groups.Count < 2)
            {
                // can't have two matching groups, so quit
                return new List<Conclusion>();
            }

            // check every possible pair
            foreach (var pair in groups.GetAllPairs())
            {
                // find the groups in the "other" direction for the matching cells
                var cellsInSourceGroup1 = pair.Item1.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Contains(digit));
                var cellsInSourceGroup2 = pair.Item2.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Contains(digit));

                var cross1 =
                    pair.Item1.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Contains(digit))
                        .Select(c => c.ContainingGroups.First(g => g.GroupType == second))
                        .ToList();
                var cross2 =
                    pair.Item2.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Contains(digit))
                        .Select(c => c.ContainingGroups.First(g => g.GroupType == second))
                        .ToList();

                // do they match?
                if (cross1.All(g => cross2.Contains(g)))
                {
                    // yes, it's an X-Wing. Now do I have useful conclusions?
                    var reason = cellsInSourceGroup1.Union(cellsInSourceGroup2).ToList();

                    var res = cross1.SelectMany(g => g.Cells)
                        .Where(c => !c.GivenOrCalculatedValue.HasValue && !c.ContainingGroups.Contains(pair.Item1) && !c.ContainingGroups.Contains(pair.Item2))
                        .Where(c => c.AvailableValues.Contains(digit))
                        .ToList();

                    if (res.Any())
                        return res.Select(c => new Conclusion(SolverType.XWing, c, Complexity, new[] { digit }, reason))
                                    .ToList();
                }
            }

            return new List<Conclusion>();
        }
    }
}
