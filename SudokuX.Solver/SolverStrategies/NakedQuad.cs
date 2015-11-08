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
    /// Find four cells in one group that contain (a subset of) 4 available values.
    /// The other cells in the group can't have these values.
    /// </summary>
    public class NakedQuad : ISolverStrategy
    {
        public float Complexity
        {
            get { return 8f; }
        }

        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            foreach (CellGroup group in grid.CellGroups)
            {
                var conclusions = FindNakedQuads(group).ToList();

                foreach (var c in conclusions)
                    yield return c;
            }
        }

        private IEnumerable<Conclusion> FindNakedQuads(CellGroup cellGroup)
        {
            var possibleQuads = cellGroup.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Count <= 4).ToList();

            if (possibleQuads.Count < 4)
                yield break;

            foreach(var possibleQuad in possibleQuads.GetAllQuads())
            {
                var availables = possibleQuad.SelectMany(q => q.AvailableValues).Distinct().ToList();
                if (availables.Count == 4)
                {
                    // this possibleQuad is a naked quad!
                    // find the cells to check outside this quad
                    // these 4 values can be erased outside this quad
                    var outside = cellGroup.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue && !possibleQuad.Contains(c)).ToList();
                    foreach(var cell in outside)
                    {
                        var toomuch = cell.AvailableValues.Intersect(availables).ToList();
                        if (toomuch.Any())
                        {
                            yield return new Conclusion(Support.Enums.SolverType.NakedQuad, cell, Complexity, toomuch, possibleQuad);
                        }
                    }
                }
            }

        }
    }
}
