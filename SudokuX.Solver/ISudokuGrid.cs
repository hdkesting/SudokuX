using System;
using System.Collections.Generic;
using SudokuX.Solver.Support;

namespace SudokuX.Solver
{
    /// <summary>
    /// A sudoku grid.
    /// </summary>
    public interface ISudokuGrid
    {
        /// <summary>
        /// Gets the size of the grid (height, width, number of symbols per group).
        /// </summary>
        /// <value>
        /// The size of the grid.
        /// </value>
        int GridSize { get; }

        /// <summary>
        /// Gets the inclusive minimum value of a grid cell.
        /// </summary>
        int MinValue { get; }

        /// <summary>
        /// Gets the inclusive maximum value of a grid cell.
        /// </summary>
        int MaxValue { get; }

        /// <summary>
        /// Gets the cell on a particular row and column.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        Cell GetCellByRowColumn(int row, int column);

        /// <summary>
        /// Gets all defined groups of cells.
        /// </summary>
        IEnumerable<CellGroup> CellGroups { get; }


        /// <summary>
        /// Are all fields either given or calculated? No blanks left?
        /// </summary>
        /// <returns></returns>
        bool IsAllKnown();

        /// <summary>
        /// Is this a valid full solution?
        /// </summary>
        /// <returns></returns>
        Validity IsChallengeDone();

        Position GetRandomEmptyPosition(Random rnd);

        IEnumerable<Cell> AllCells();

        double GetPercentageDone();

    }
}
