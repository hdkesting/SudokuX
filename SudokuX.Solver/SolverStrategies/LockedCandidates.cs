using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.SolverStrategies
{
    /// <summary>
    /// A digit can be placed on just a limited number of cells in one group and all those cells also belong to the same other group.
    /// That digit can't occur on other cells of that other group.
    /// This is an interaction between a block and a row, column or diagonal.
    /// </summary>
    public class LockedCandidates : ISolverStrategy
    {
        /// <summary>
        /// Processes the grid and returns any helpful conclusions.
        /// </summary>
        /// <param name="grid">The grid to process.</param>
        /// <returns></returns>
        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            for (int i = grid.MinValue; i <= grid.MaxValue; i++)
            {
                var conclusions = EvaluateCandidates(i, grid);
                foreach (var c in conclusions)
                    yield return c;
            }
        }

        /// <summary>
        /// Gets the complexity-score of this solver (6).
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        public int Complexity
        {
            get { return 6; }
        }

        private IList<Conclusion> EvaluateCandidates(int digit, ISudokuGrid grid)
        {
            // check all "block" groups
            foreach (var cellGroup in grid.CellGroups.Where(g => g.GroupType == GroupType.Block || g.GroupType == GroupType.SpecialBlock))
            {
                // in what cells is this digit a possible value?
                var cellswithdigit =
                    cellGroup.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Contains(digit)).ToList();

                // found more than one cell (if one, then it should have been caught as NakedSingle)
                if (cellswithdigit.Count > 1)
                {
                    // are there other non-block groups containing all these cells?
                    var groups = GetJointGroups(cellswithdigit).ToList();
                    groups.Remove(cellGroup); // remove the group we started with

                    if (groups.Any())
                    {
                        // any cells in those other groups where this digit is still a candidate?
                        var candidates = FindCandidates(digit, groups, cellGroup);
                        if (candidates.Any())
                        {
                            return candidates;
                        }
                    }
                }
            }

            return new List<Conclusion>();
        }

        private IList<Conclusion> FindCandidates(int digit, IEnumerable<CellGroup> groups, CellGroup sourceGroup)
        {
            List<Conclusion> conclusions = new List<Conclusion>();
            foreach (Cell cell in groups.SelectMany(g => g.Cells))
            {
                // not in the original group,
                // doesn't have a value yet
                // and contains this as a possible value
                if (!cell.ContainingGroups.Contains(sourceGroup) && !cell.GivenOrCalculatedValue.HasValue && cell.AvailableValues.Contains(digit))
                {
                    conclusions.Add(new Conclusion(SolverType.LockedCandidates, cell, Complexity, new[] { digit }, sourceGroup.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue)));
                }
            }

            return conclusions;
        }

        /// <summary>
        /// Gets all groups where all these cells belong to. This includes the original group.
        /// </summary>
        /// <param name="cells">The cells.</param>
        /// <returns></returns>
        private static IEnumerable<CellGroup> GetJointGroups(IList<Cell> cells)
        {
            var groups = new List<CellGroup>();
            groups.AddRange(cells.First().ContainingGroups);

            foreach (Cell cell in cells.Skip(1))
            {
                groups = groups.Intersect(cell.ContainingGroups).ToList();
            }

            return groups;
        }
    }
}
