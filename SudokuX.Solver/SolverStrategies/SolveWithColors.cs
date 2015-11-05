using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.SolverStrategies
{
    /// <summary>
    /// "Solve with colors"
    /// </summary>
    internal class SolveWithColors : ISolverStrategy
    {
        private enum CellColor { Green, Blue }

        /// <summary>
        /// Gets the complexity-score of this solver (7).
        /// </summary>
        /// <value>
        /// The complexity.
        /// </value>
        public float Complexity
        {
            get { return 7f; }
        }

        /*
         * repeat for all candidate value
         * repeat for all blocks containing this value as cell-candidate in exactly two cells
         * make 1 "green"
         * for all groups of that cell that contain 1 other candidate: make those others "blue" (if they are not already colored blue)
         * for all those made blue, repeat and make the others green (if not already)
         * etc, until
         * - no more candidates to process
         * - clash: there is another sibling cell with the same color - then that color (every cell) is wrong
         * finally: check all uncolored cells: if they have siblings with both colors, then the original cannot contain that value
         */

        /// <summary>
        /// Processes the grid and returns any helpful conclusions.
        /// </summary>
        /// <param name="grid">The grid to process.</param>
        /// <returns></returns>
        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            // check for every possible value
            for (var candidateValue = grid.MinValue; candidateValue <= grid.MaxValue; candidateValue++)
            {
                var conclusions = ProcessGridBlocks(grid, candidateValue);
                if (conclusions.Any())
                    return conclusions;
            }

            return Enumerable.Empty<Conclusion>();
        }

        private IList<Conclusion> ProcessGridBlocks(ISudokuGrid grid, int candidateValue)
        {
            foreach (var block in grid.CellGroups.Where(g => g.GroupType == GroupType.Block))
            {
                var cells =
                    block.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Contains(candidateValue))
                        .ToList();
                // I need to start with blocks containing exactly two possibilities
                if (cells.Count == 2)
                {
                    // make a fresh copy of the colorgrid
                    var colorgrid = new CellColor?[grid.GridSize, grid.GridSize];
                    var queue = new Queue<Cell>();
                    var cell = cells.First();
                    SetColor(colorgrid, cell, CellColor.Green);
                    queue.Enqueue(cell);

                    // process the queue to color all siblings and theirs
                    ProcessQueue(queue, colorgrid, candidateValue);

                    // return conclusions (if any)
                    var list = CheckConclusions(grid, colorgrid, candidateValue, cells);
                    if (list.Any())
                    {
                        return list;
                    }
                }
            }

            return Enumerable.Empty<Conclusion>().ToList();
        }

        private IList<Conclusion> CheckConclusions(ISudokuGrid grid, CellColor?[,] colorgrid, int candidateValue, IList<Cell> startpair)
        {
            var result = new List<Conclusion>();
            // any group with two cells having the same color? -> the start cell of that color is NOT this number
            foreach (var cellGroup in grid.CellGroups)
            {
                var colored = cellGroup.Cells
                        .Where(c => GetColor(colorgrid, c).HasValue)
                        .GroupBy(c => GetColor(colorgrid, c).Value)
                        .ToList();
                // correct: two groups (blue and green), both containing one cell
                // also correct: one group (blue or green), containing one cell
                // error: a group contains 2 or more cells - that color is wrong, so the other is right!

                var wrong = colored.Where(g => g.Count() > 1).ToList();
                if (wrong.Any())
                {
                    var wrongcolor = wrong.First().Key;
                    var correctcell = startpair.Single(c => GetColor(colorgrid, c) != wrongcolor);
                    result.Add(new Conclusion(Support.Enums.SolverType.SolveWithColors, correctcell, Complexity, candidateValue, startpair));
                    return result; // return just this one
                }
            }

            // check each non-colored cell, if it is in a group with both colors, then it can't be a valid candidate
            foreach (var cell in grid.AllCells().Where(c => !c.GivenOrCalculatedValue.HasValue
                                                                && c.AvailableValues.Contains(candidateValue)
                                                                && !GetColor(colorgrid, c).HasValue))
            {
                foreach (var cellGroup in cell.ContainingGroups)
                {
                    var allsibs = cellGroup.Cells
                                    .Where(c => !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Contains(candidateValue))
                                    .ToList();

                    var colors = allsibs.Select(c => GetColor(colorgrid, c)).Where(col => col.HasValue).Distinct().ToList();

                    if (colors.Count == 2)
                    {
                        // this uncolored cell has siblings of both colors. It is no candidate.
                        result.Add(new Conclusion(Support.Enums.SolverType.SolveWithColors, cell, Complexity, new[] { candidateValue }, startpair));
                    }
                }
            }

            return result;
        }

        private void ProcessQueue(Queue<Cell> queue, CellColor?[,] colorgrid, int candidateValue)
        {
            while (queue.Count != 0)
            {
                var cell = queue.Dequeue();
                var othercolor = GetColor(colorgrid, cell) == CellColor.Green ? CellColor.Blue : CellColor.Green;

                foreach (var cellGroup in cell.ContainingGroups)
                {
                    var sibs =
                        cellGroup.Cells.Where(
                            c => c != cell && !c.GivenOrCalculatedValue.HasValue && c.AvailableValues.Contains(candidateValue)).ToList();
                    // I want groups containing exactly one other possibility
                    if (sibs.Count == 1)
                    {
                        var sib = sibs.Single();
                        if (!GetColor(colorgrid, sib).HasValue)
                        {
                            // doesn't have a color yet, so give it the other color
                            SetColor(colorgrid, sib, othercolor);
                            queue.Enqueue(sib);
                        }
                        // ignore if it already has a color, even if it's the same one (which is wrong but dealt with later)
                    }
                }
            }
        }

        private static CellColor? GetColor(CellColor?[,] colorgrid, Cell cell)
        {
            return colorgrid[cell.Row, cell.Column];
        }

        private static void SetColor(CellColor?[,] colorgrid, Cell cell, CellColor newcolor)
        {
            colorgrid[cell.Row, cell.Column] = newcolor;
        }
    }


}
