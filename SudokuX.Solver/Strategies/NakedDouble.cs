using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Strategies
{
    /// <summary>
    /// Within a group, check for two cells with only (the same) two possibilities: the other cells in this group can't have these two.
    /// </summary>
    public class NakedDouble : ISolver
    {
        private const int Complexity = 3;

        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            Debug.WriteLine("Invoking NakedDouble");
            foreach (var group in grid.CellGroups)
            {
                var list = FindNakedDoubles(group).ToList();
                if (list.Any())
                {
                    return list;
                }
            }

            return Enumerable.Empty<Conclusion>();
        }

        private IEnumerable<Conclusion> FindNakedDoubles(CellGroup cellGroup)
        {
            var doubles = cellGroup.Cells.Where(c => !c.HasValue && c.AvailableValues.Count == 2).ToList();

            // if you've processed a->b, then there's no need to process b->a

            while (doubles.Count >= 2)
            {
                var localcell = doubles.First();
                doubles.Remove(localcell);
                if (localcell.AvailableValues.Count < 2)
                {
                    continue;
                }

                foreach (var possibletwin in doubles)
                {
                    if (possibletwin.AvailableValues.Count < 2)
                    {
                        continue; // one of the two values has been removed by a previous naked-double in this group
                    }

                    var localtwin = possibletwin;
                    if (!localcell.AvailableValues.Except(localtwin.AvailableValues).Any())
                    {

                        // cellen hebben dezelfde twee mogelijke waarden
                        // dus de anderen uit de groep hebben die waarden niet

                        var list = GetExclusions(localcell, localtwin).ToList();
                        if (list.Any())
                        {
                            Debug.WriteLine("Naked double: found values {0} and {1} in cells {2} and {3} (group {4})",
                                localcell.AvailableValues[0], localcell.AvailableValues[1],
                                localcell, localtwin,
                                cellGroup);
                            return list;
                        }
                    }
                }
            }
            return Enumerable.Empty<Conclusion>();
        }

        private IEnumerable<Conclusion> GetExclusions(Cell cell1, Cell cell2)
        {
            var commongroups = cell1.ContainingGroups.Intersect(cell2.ContainingGroups).ToList();
            var values = cell1.AvailableValues.Union(cell2.AvailableValues).ToList();

            foreach (var cell in commongroups.SelectMany(g => g.Cells).Where(c => c != cell1 && c != cell2).Distinct())
            {
                if (!cell.HasValue)
                {
                    var todo = cell.AvailableValues.Intersect(values).ToList();
                    if (todo.Any())
                    {
                        yield return new Conclusion(cell, Complexity, todo);
                    }
                }
            }
        }
    }
}
