using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Strategies
{
    /// <summary>
    /// Find a set of two numbers that are only in two cells: the other numbers in those two cells are not valid
    /// </summary>
    public class HiddenDouble : ISolver
    {
        private const int Complexity = 3;

        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            Debug.WriteLine("Invoking HiddenDouble");
            foreach (var group in grid.CellGroups)
            {
                var list = FindHiddenDoubles(group, grid.MinValue, grid.MaxValue).ToList();
                if (list.Any())
                {
                    // use just the first batch of conclusions
                    return list;
                }
            }
            return Enumerable.Empty<Conclusion>();
        }

        private IEnumerable<Conclusion> FindHiddenDoubles(CellGroup group, int min, int max)
        {
            for (int v1 = min; v1 < max; v1++)
            {
                if (group.Cells.Any(c => c.GivenValue == v1 || c.CalculatedValue == v1))
                    continue; // already known - forget about it

                for (int v2 = v1 + 1; v2 <= max; v2++)
                {
                    if (group.Cells.Any(c => c.GivenValue == v2 || c.CalculatedValue == v2))
                        continue; // already known - forget about it

                    // v1 < v2
                    int[] potentialDouble = { v1, v2 };

                    int count = group.Cells.Count(cell => !cell.HasValue && cell.AvailableValues.Intersect(potentialDouble).Any());

                    if (count == 2)
                    {
                        var pair = group.Cells.Where(cell => !cell.HasValue && cell.AvailableValues.Intersect(potentialDouble).Any()).ToArray();

                        var concl1 = new Conclusion(pair[0], Complexity, pair[0].AvailableValues.Except(potentialDouble));
                        var concl2 = new Conclusion(pair[1], Complexity, pair[1].AvailableValues.Except(potentialDouble));

                        if (concl1.ExcludedValues.Any() || concl2.ExcludedValues.Any())
                        {
                            Debug.WriteLine("Hidden double found: {0} and {1} in group {2}", potentialDouble[0], potentialDouble[1], group);

                            if (concl1.ExcludedValues.Any())
                                yield return concl1;
                            if (concl2.ExcludedValues.Any())
                                yield return concl2;
                        }
                    }
                }
            }
        }
    }
}
