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

        public int Complexity
        {
            get { return 3; }
        }

        private IEnumerable<Conclusion> FindNakedDoubles(CellGroup cellGroup)
        {
            // find all cells in the group with exactly two options left
            var doubles = cellGroup.Cells.Where(c => !c.HasGivenOrCalculatedValue && c.AvailableValues.Count == 2).ToList();

            // if you've processed a->b, then there's no need to process b->a

            while (doubles.Count >= 2)
            {
                var localcell = doubles.First();
                doubles.Remove(localcell);

                foreach (var possibletwin in doubles)
                {
                    var localtwin = possibletwin;
                    if (localcell.AvailableValues.All(v => localtwin.AvailableValues.Contains(v)))
                    {
                        // these two cells share the same two possible values
                        // so the rest in this group can't have these values

                        var list = GetExclusions(localcell, localtwin).ToList();
                        if (list.Any())
                        {
                            Debug.WriteLine("Naked double: found values {0} and {1} in cells {2} and {3} (group {4})",
                                localcell.AvailableValues[0], localcell.AvailableValues[1],
                                localcell, localtwin,
                                cellGroup);
                            foreach (var conclusion in list)
                            {
                                yield return conclusion;
                            }
                        }
                    }
                }
            }
            //return Enumerable.Empty<Conclusion>();
        }

        private IEnumerable<Conclusion> GetExclusions(Cell cell1, Cell cell2)
        {
            // all groups that contain these two cells:
            var commongroups = cell1.ContainingGroups.Intersect(cell2.ContainingGroups).ToList();

            // the values of the double:
            //var values = cell1.AvailableValues.Union(cell2.AvailableValues).ToList();
            var values = cell1.AvailableValues; // identical to cell2.AvailableValues

            // check all sibling cells for options to remove: find cells that have these values as "available" - that should be removed
            var cells = commongroups
                    .SelectMany(g => g.Cells)
                    .Where(c => !c.HasGivenOrCalculatedValue && c != cell1 && c != cell2)
                    .Distinct()
                    .ToList();

            foreach (var sibling in cells)
            {
                var todo = sibling.AvailableValues.Intersect(values).ToList();
                if (todo.Any())
                {
                    yield return new Conclusion(sibling, Complexity, todo);
                }
            }
        }
    }
}
