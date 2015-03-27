using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Strategies
{
    /// <summary>
    /// Finds cells that are the only one within a group where some particular value can be.
    /// </summary>
    public class HiddenSingle : ISolver
    {
        private const int Complexity = 1;

        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            Debug.WriteLine("Invoking HiddenSingle");
            var list1 = grid.CellGroups
                .SelectMany(g => HiddenSinglesInGroup(g, grid.MinValue, grid.MaxValue))
                .Distinct()
                .ToList();

            return list1;
        }

        private IEnumerable<Conclusion> HiddenSinglesInGroup(CellGroup group, int min, int max)
        {
            for (int val = min; val <= max; val++)
            {
                int found = 0;
                Cell latest = null;
                foreach (var cell in group.Cells)
                {
                    if (cell.CalculatedValue == val || cell.GivenValue == val)
                    {
                        // already have given this value, it can't be a hidden single
                        latest = null;
                        break; // foreach
                    }

                    if (!cell.HasValue && cell.AvailableValues.Contains(val))
                    {
                        // found this value as available - but it might be elsewhere also
                        latest = cell;
                        found++;
                    }
                }

                if (latest != null && found == 1)
                {
                    // just one possibility found!
                    Debug.WriteLine("Found hidden single {0} in cell {1} in group {2}", val, latest, group);
                    yield return new Conclusion(latest, Complexity) { ExactValue = val };
                }
            }
        }
    }
}
