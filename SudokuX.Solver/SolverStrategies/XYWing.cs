using SudokuX.Solver.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.SolverStrategies
{
    /// <summary>
    /// Search for a cell with two candidates (x,y). In all it's groups, search for another cell with two candidates AND one matching one (x,z).
    /// From there search for a third cell with (y,z) as only candidates. In the cells that share a group with the first and third cell, 
    /// the y is not a valid candidate.
    /// </summary>
    public class XYWing : ISolverStrategy
    {
        public int Complexity
        {
            get { return 9; }
        }

        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            foreach (var firstcell in grid.AllCells().Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Count == 2))
            {
                var firstsibs = firstcell.ContainingGroups.SelectMany(g => g.Cells).Where(c => c != firstcell && !c.GivenOrCalculatedValue.HasValue).ToList();
                foreach (var candidate in FindCandidates(grid, firstcell))
                {
                    var thirdcell = candidate.Item1;
                    var todelete = candidate.Item3;

                    var thirdsibs = thirdcell.ContainingGroups.SelectMany(g => g.Cells).Where(c => c != thirdcell && !c.GivenOrCalculatedValue.HasValue).ToList();
                    var matchcells = thirdsibs.Intersect(firstsibs).Where(c => c.AvailableValues.Contains(todelete)).ToList();

                    if (matchcells.Any())
                    {
                        foreach (var cell in matchcells)
                        {
                            yield return new Conclusion(Support.Enums.SolverType.XYWing, cell, Complexity, new[] { todelete }, new[] { firstcell, candidate.Item2, thirdcell });
                        }

                        yield break; // quit after first?
                    }
                }
            }
        }

        private IEnumerable<Tuple<Cell, Cell, int>> FindCandidates(ISudokuGrid grid, Cell first)
        {
            // available in first: x,y
            // available in second: y,z
            // so third must be: x,z to complete the XY-Wing
            foreach (var second in first.ContainingGroups
                                            .SelectMany(g => g.Cells)
                                            .Where(c => c != first)
                                            .Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Count == 2))
            {
                var same12 = first.AvailableValues.Intersect(second.AvailableValues).ToList();
                if (same12.Count == 1)
                {
                    var valY = same12.Single();
                    var valX = first.AvailableValues.Where(v => v != valY).Single();
                    var valZ = second.AvailableValues.Where(v => v != valY).Single();

                    foreach (var third in second.ContainingGroups
                                                    .SelectMany(g => g.Cells)
                                                    .Where(c => c != first && c != second)
                                                    .Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Count == 2))
                    {
                        if (third.AvailableValues.Contains(valX) && third.AvailableValues.Contains(valZ))
                        {
                            yield return Tuple.Create(third, second, valX);
                        }
                    }
                }
            }
        }
    }
}
