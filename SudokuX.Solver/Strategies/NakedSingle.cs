using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Strategies
{
    /// <summary>
    /// Finds unfilled cells with exactly one possibility.
    /// </summary>
    public class NakedSingle : ISolver
    {
        private const int Complexity = 0;

        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            Debug.WriteLine("Invoking NakedSingle");
            //var list = new List<Conclusion>();

            //for (int r = 0; r < grid.GridSize; r++)
            //{
            //    for (int c = 0; c < grid.GridSize; c++)
            //    {
            //        var cell = grid.GetCellByRowColumn(r, c);
            //        if (!cell.HasValue && cell.AvailableValues.Count() == 1)
            //        {
            //            Debug.WriteLine("Found naked single {0} in cell {1}", cell.AvailableValues.Single(), cell);
            //            list.Add(new Conclusion(cell, Complexity) { ExactValue = cell.AvailableValues.Single() });
            //        }
            //    }
            //}

            var list = grid.AllCells().ToList()
                .Where(c => !c.HasValue && c.AvailableValues.Count() == 1)
                .Select(c =>
                {
                    Debug.WriteLine("Found naked single {0} in cell {1}", c.AvailableValues.Single(), c);
                    return new Conclusion(c, Complexity) { ExactValue = c.AvailableValues.Single() };
                })
                .ToList();

            return list;
        }
    }
}
