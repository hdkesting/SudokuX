using SudokuX.Solver.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.SolverStrategies
{
    /// <summary>
    /// A set of 4 digits that only appear in 4 cells of one group.
    /// The other availables in those 4 cells can be removed.
    /// </summary>
    public class HiddenQuad : ISolverStrategy
    {
        public float Complexity
        {
            get { return 9f; }
        }

        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            var result = new List<Conclusion>();
            foreach (var group in grid.CellGroups)
            {
                var list = FindHiddenQuads(group, grid.MinValue, grid.MaxValue).ToList();
                result.AddRange(list);
            }

            return result;
        }

        private IEnumerable<Conclusion> FindHiddenQuads(CellGroup cellGroup, int minValue, int maxValue)
        {
            var knownvals = cellGroup.Cells.Where(c => c.GivenOrCalculatedValue.HasValue).Select(c => c.GivenOrCalculatedValue.Value).ToList();

            if (knownvals.Count >= maxValue - minValue - 3)
            {
                // 4 open cells or less? never mind.
                yield break;
            }

            foreach (var quad in GetQuads(minValue, maxValue, knownvals))
            {
                var cells =
                    cellGroup.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Any(v => quad.Contains(v))).ToList();
                if (cells.Count == 4)
                {
                    // exactly 4 cells with any of the four quad values - this is a quad, possibly hidden by other availables
                    var reason = cellGroup.Cells.Except(cells).ToList();
                    for (int i = 0; i < 4; i++)
                    {
                        var extravalues = cells[i].AvailableValues.Where(v => !quad.Contains(v)).ToList();
                        if (extravalues.Any())
                        {
                            yield return new Conclusion(Support.Enums.SolverType.HiddenQuad, cells[i], Complexity, extravalues, reason);
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Get quads that do not contain any of the known values.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="knowVals"></param>
        /// <returns></returns>
        private IEnumerable<IList<int>> GetQuads(int minValue, int maxValue, IList<int> knowVals)
        {
            return EnumerableExtensions.GetSubsets(minValue, maxValue, 4)
                .Where(l => l.All(i => !knowVals.Contains(i)));
        }

    }
}
