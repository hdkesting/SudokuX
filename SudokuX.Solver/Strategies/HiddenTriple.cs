using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Strategies
{
    /// <summary>
    /// Find a set of three numbers that are only in three cells within the group. The other numbers in those cells are invalid.
    /// Note that not all three values need to be in all three cells.
    /// </summary>
    public class HiddenTriple : ISolver
    {
        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            var result = new List<Conclusion>();
            foreach (var group in grid.CellGroups)
            {
                var list = FindHiddenTriples(group, grid.MinValue, grid.MaxValue).ToList();
                result.AddRange(list);
            }

            return result;
        }

        public int Complexity
        {
            get { return 6; }
        }

        private IEnumerable<Conclusion> FindHiddenTriples(CellGroup cellGroup, int minValue, int maxValue)
        {
            // find triplet values to ignore
            var knownvals =
                cellGroup.Cells.Where(c => c.HasGivenOrCalculatedValue).Select(c => c.GivenValue ?? c.CalculatedValue).ToList();
            if (knownvals.Count >= maxValue - minValue - 2)
            {
                // 3 open cells or less? never mind.
                return Enumerable.Empty<Conclusion>();
            }

            foreach (var triplet in GetTriplets(minValue, maxValue))
            {
                if (triplet.Any(x => knownvals.Contains(x)))
                {
                    // triplet contains a given or calculated value - skip
                    continue;
                }

                var cells =
                    cellGroup.Cells.Where(c => !c.HasGivenOrCalculatedValue && c.AvailableValues.Any(v => triplet.Contains(v))).ToList();
                if (cells.Count == 3)
                {
                    // exactly 3 cells with any of the three triplet values - this is a triplet, possibly hidden
                    var result = new List<Conclusion>();
                    for (int i = 0; i < 3; i++)
                    {
                        var vals = cells[i].AvailableValues.Where(v => !triplet.Contains(v)).ToList();
                        if (vals.Any())
                        {
                            result.Add(new Conclusion(cells[i], Complexity, vals));
                        };
                    }

                    if (result.Any())
                    {
                        return result; // return the first real result of this group, no need to check further.
                    }
                }
            }

            return Enumerable.Empty<Conclusion>();
        }

        private IEnumerable<IList<int>> GetTriplets(int minValue, int maxValue)
        {
            for (int i1 = minValue; i1 <= maxValue - 2; i1++)
            {
                for (int i2 = i1 + 1; i2 <= maxValue - 1; i2++)
                {
                    for (int i3 = i2 + 1; i3 <= maxValue; i3++)
                    {
                        yield return new[] { i1, i2, i3 };
                    }
                }
            }
        }
    }
}
