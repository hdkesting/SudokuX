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

        public int Complexity
        {
            get { return 7; }
        }

        /*
         * repeat for all candidate value
         * repeat for all blocks containing this value as cell-candidate in exactly two cells
         * make 1 "green"
         * for all groups of that cell that contain 1 other candidate: make those others "blue" (if they are not already colored blue)
         * for all those made blue, repeat and make the others green (if not already)
         * etc, until
         * - no more candidates to process
         * - clash: a colored cell should get a different color
         */

        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
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
                    block.Cells.Where(c => !c.HasGivenOrCalculatedValue && c.AvailableValues.Contains(candidateValue))
                        .ToList();
                if (cells.Count == 2)
                {
                    // make a fresh copy of the colorgrid
                    var colorgrid = new CellColor?[grid.GridSize, grid.GridSize];
                    var queue = new Queue<Cell>();
                    var cell = cells.First();
                    colorgrid[cell.Row, cell.Column] = CellColor.Green;
                    queue.Enqueue(cell);

                    // process the queue
                    var success = ProcessQueue(queue, colorgrid, candidateValue);

                    // return conclusions (if any)
                    if (success)
                    {
                        var list = CheckConclusions(grid, colorgrid, candidateValue, cells);
                        if (list.Any())
                        {
                            return list;
                        }
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
                var colored = cellGroup.Cells.Where(c => GetColor(colorgrid, c).HasValue).ToList();
                if (colored.Count == 2)
                {
                    if (GetColor(colorgrid, colored[0]) == GetColor(colorgrid, colored[1]))
                    {
                        var wrongcell = startpair.Single(c => GetColor(colorgrid, c) == GetColor(colorgrid, colored[0]));
                        result.Add(new Conclusion(wrongcell, Complexity));
                        return result; // return just this one
                    }
                }
            }

            // check each non-colored cell, if it is in a group with both colors, then it can't be a valid candidate
            foreach (var cell in grid.AllCells().Where(c => !c.HasGivenOrCalculatedValue
                                                                && c.AvailableValues.Contains(candidateValue)
                                                                && !GetColor(colorgrid, c).HasValue))
            {
                foreach (var cellGroup in cell.ContainingGroups)
                {
                    var allsibs = cellGroup.Cells
                                    .Where(c => c.AvailableValues.Contains(candidateValue))
                                    .ToList();

                    var colors = allsibs.Select(c => GetColor(colorgrid, c)).Where(col => col.HasValue).Distinct().ToList();

                    if (colors.Count == 2)
                    {
                        // this uncolored cell has siblings of both colors. It is no candidate.
                        result.Add(new Conclusion(cell, Complexity, new[] { candidateValue }));
                    }
                }

            }

            return result;
        }

        private bool ProcessQueue(Queue<Cell> queue, CellColor?[,] colorgrid, int candidateValue)
        {
            while (queue.Count != 0)
            {
                var cell = queue.Dequeue();
                var othercolor = GetColor(colorgrid, cell) == CellColor.Green ? CellColor.Blue : CellColor.Green;

                foreach (var cellGroup in cell.ContainingGroups)
                {
                    var sibs =
                        cellGroup.Cells.Where(
                            c => c != cell && !c.HasGivenOrCalculatedValue && c.AvailableValues.Contains(candidateValue)).ToList();
                    if (sibs.Count == 1)
                    {
                        var sib = sibs.Single();
                        if (!GetColor(colorgrid, sib).HasValue)
                        {
                            colorgrid[sib.Row, sib.Column] = othercolor;
                            queue.Enqueue(sib);
                        }
                        else if (GetColor(colorgrid, sib) != othercolor)
                        {
                            // error!
                            return false;
                        }
                        // else: skip, already has the correct color, no need to revisit it (or it's siblings)
                    }
                }
            }

            return true;
        }

        private static CellColor? GetColor(CellColor?[,] colorgrid, Cell cell)
        {
            return colorgrid[cell.Row, cell.Column];
        }
    }


}
