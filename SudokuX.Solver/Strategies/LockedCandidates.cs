using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Strategies
{
    /// <summary>
    /// A digit can be placed on just a limited number of cells in one group and all those cells also belong to the same other group.
    /// That digit can't occur on other cells of that other group.
    /// </summary>
    public class LockedCandidates : ISolver
    {
        private const int Complexity = 6;

        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            Debug.WriteLine("Invoking LockedCandidates");
            for (int i = grid.MinValue; i <= grid.MaxValue; i++)
            {
                var conclusions = EvaluateCandidates(i, grid);
                if (conclusions.Any())
                {
                    return conclusions;
                }
            }

            return Enumerable.Empty<Conclusion>();
        }

        private static IList<Conclusion> EvaluateCandidates(int digit, ISudokuGrid grid)
        {
            foreach (var cellGroup in grid.CellGroups)
            {
                var cellswithdigit =
                    cellGroup.Cells.Where(c => !c.HasValue && c.AvailableValues.Contains(digit)).ToList();

                if (cellswithdigit.Count > 1 /*&& cellswithdigit.Count <= 3*/)
                {
                    var groups = GetJointGroups(cellswithdigit).Where(g => g != cellGroup).ToList();

                    if (groups.Any())
                    {
                        var candidates = FindCandidates(digit, groups, cellGroup);
                        if (candidates.Any())
                        {
                            Debug.WriteLine("LockedCandidates: found it for {0} in group {1}", digit, cellGroup);
                            return candidates;
                        }
                    }
                }
            }

            return new List<Conclusion>();
        }

        private static IList<Conclusion> FindCandidates(int digit, IEnumerable<CellGroup> groups, CellGroup sourceGroup)
        {
            List<Conclusion> conclusions = new List<Conclusion>();
            foreach (CellGroup cellGroup in groups)
            {
                foreach (Cell cell in cellGroup.Cells)
                {
                    if (!cell.ContainingGroups.Contains(sourceGroup) && !cell.HasValue && cell.AvailableValues.Contains(digit))
                    {
                        conclusions.Add(new Conclusion(cell, Complexity, new[] { digit }));
                    }
                }
            }

            return conclusions;
        }

        private static IEnumerable<CellGroup> GetJointGroups(IList<Cell> cells)
        {
            var groups = new List<CellGroup>();
            groups.AddRange(cells.First().ContainingGroups);

            foreach (Cell cell in cells)
            {
                groups = groups.Intersect(cell.ContainingGroups).ToList();
            }

            return groups; // should have at least one value
        }
    }
}
