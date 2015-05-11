﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuX.Solver.Support;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Strategies
{
    /// <summary>
    /// X-Wing pattern: one digit in two different rows (or columns) at exactly two positions in the same columns (or rows).
    /// Then the other cells in that column (or row) can't have this digit.
    /// </summary>
    public class XWing : ISolver
    {
        public int Complexity
        {
            get { return 8; }
        }

        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            for (int digit = grid.MinValue; digit <= grid.MaxValue; digit++)
            {
                var result = FindXWing(digit, grid, GroupType.Row, GroupType.Column);
                if (result.Any())
                    return result;

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
                                .Where(g => g.Cells.Count(c => !c.HasGivenOrCalculatedValue && c.AvailableValues.Contains(digit)) == 2)
                                .ToList();
            if (groups.Count < 2)
            {
                // can't have two matching groups, so quit
                return new List<Conclusion>();
            }

            // check every possible pair
            foreach (var pair in groups.GetAllPairs())
            {
                // find the "other" groups for the matching cells
                var cross1 =
                    pair.Item1.Cells.Where(c => !c.HasGivenOrCalculatedValue && c.AvailableValues.Contains(digit))
                        .Select(c => c.ContainingGroups.First(g => g.GroupType == second))
                        .ToList();
                var cross2 =
                    pair.Item2.Cells.Where(c => !c.HasGivenOrCalculatedValue && c.AvailableValues.Contains(digit))
                        .Select(c => c.ContainingGroups.First(g => g.GroupType == second))
                        .ToList();

                // do they match?
                if (cross1.All(g => cross2.Contains(g)))
                {
                    // yes, it's an X-Wing. Now do I have useful conclusions?
                    var res = cross1.SelectMany(g => g.Cells)
                        .Where(c => !c.ContainingGroups.Contains(pair.Item1) && !c.ContainingGroups.Contains(pair.Item2))
                        .Where(c => c.AvailableValues.Contains(digit))
                        .ToList();

                    if (res.Any())
                        return res.Select(c => new Conclusion(c, Complexity, new[] { digit }))
                                    .ToList();
                }
            }

            return new List<Conclusion>();
        }
    }
}
