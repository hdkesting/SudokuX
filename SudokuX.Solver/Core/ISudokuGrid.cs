using System;
using System.Collections.Generic;
using SudokuX.Solver.Support;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Core
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
        Validity CalculateValidity();

        /// <summary>
        /// Gets a list of empty positions.
        /// </summary>
        /// <param name="rnd">The random generator.</param>
        /// <returns></returns>
        IEnumerable<Position> GetEmptyPositions(Random rnd);

        /// <summary>
        /// Enumerates all the cells.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Cell> AllCells();

        /// <summary>
        /// Gets the percentage done.
        /// </summary>
        /// <returns></returns>
        double GetPercentageDone();

        /// <summary>
        /// Clones the board, preserving size, blocks and challenge values.
        /// </summary>
        /// <returns></returns>
        ISudokuGrid CloneBoardAsChallenge();

        /// <summary>
        /// Gets a value indicating whether this grid is regular.
        /// </summary>
        /// <value>
        /// <c>true</c> if this grid is regular; otherwise, <c>false</c>.
        /// </value>
        bool IsRegular { get; }

        /// <summary>
        /// Gets a value indicating whether this grid has special groups.
        /// </summary>
        /// <value>
        /// <c>true</c> if this grid has special groups; otherwise, <c>false</c>.
        /// </value>
        bool HasSpecialGroups { get; }

    }
}
