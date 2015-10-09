using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// A basic N x N grid, without blocks
    /// </summary>
    public abstract class BasicGrid : ISudokuGrid
    {
        private readonly List<CellGroup> _rows = new List<CellGroup>();
        private readonly List<CellGroup> _cols = new List<CellGroup>();

        private readonly Cell[,] _grid;

        /// <summary>
        /// Gets the size of the grid.
        /// </summary>
        public int GridSize { get; private set; }

        /// <summary>
        /// Gets the inclusive minimum value of a grid cell.
        /// </summary>
        public int MinValue { get; private set; }

        /// <summary>
        /// Gets the grid.
        /// </summary>
        /// <value>
        /// The grid.
        /// </value>
        protected Cell[,] Grid { get { return _grid; } }

        /// <summary>
        /// Gets the inclusive maximum value of a grid cell.
        /// </summary>
        public int MaxValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicGrid"/> class.
        /// </summary>
        /// <param name="size">The size.</param>
        protected BasicGrid(int size)
        {
            _grid = new Cell[size, size];

            GridSize = size;
            if (size < 13)
            {
                MinValue = 1;
                MaxValue = GridSize;
            }
            else
            {
                MinValue = 0;
                MaxValue = GridSize - 1;
            }

            Initialize();
        }

        /// <summary>
        /// Initializes the rows and columns.
        /// </summary>
        private void Initialize()
        {
            // create groups
            for (int i = 0; i < GridSize; i++)
            {
                _rows.Add(new CellGroup(GridSize, i) { Name = "Row " + i, GroupType = GroupType.Row });
                _cols.Add(new CellGroup(GridSize, i) { Name = "Column " + i, GroupType = GroupType.Column });
            }

            // add cells to row and column groups and to grid
            for (int r = 0; r < GridSize; r++)
            {
                var row = GetRowByOrdinal(r);
                for (int c = 0; c < GridSize; c++)
                {
                    var col = GetColumnByOrdinal(c);

                    var cell = new Cell(r, c, MinValue, MaxValue)
                    {
                        Name = String.Format("r {0}, c {1}", r, c)
                    };
                    _grid[r, c] = cell;
                    cell.AddToGroups(row, col);
                }
            }
        }

        /// <summary>
        /// Gets the cell on a particular row and column.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public Cell GetCellByRowColumn(int row, int column)
        {
            return _grid[row, column];
        }

        /// <summary>
        /// Gets all defined groups of cells.
        /// </summary>
        public virtual IEnumerable<CellGroup> CellGroups
        {
            get
            {
                foreach (var cellGroup in _rows)
                {
                    yield return cellGroup;
                }
                foreach (var cellGroup in _cols)
                {
                    yield return cellGroup;
                }
            }
        }

        /// <summary>
        /// Are all fields either given or calculated? No blanks left?
        /// </summary>
        /// <returns></returns>
        public bool IsAllKnown()
        {
            return AllCells().All(c => c.GivenOrCalculatedValue.HasValue);
        }

        /// <summary>
        /// Is this a valid full solution?
        /// </summary>
        /// <returns></returns>
        public Validity IsChallengeDone()
        {
            Validity result = Validity.Full;
            if (AllCells().Any(c => !c.GivenOrCalculatedValue.HasValue && !c.AvailableValues.Any()))
            {
                //Debug.WriteLine("IsChallengeDone: No. No availables left in some empty cell.");
                return Validity.Invalid;
            }

            foreach (var cellGroup in CellGroups)
            {
                var values = cellGroup.Cells.Select(c => c.GivenValue ?? c.CalculatedValue).Where(c => c.HasValue).ToList();
                int count = values.Distinct().Count();
                if (count != values.Count)
                {
                    //Debug.WriteLine("IsChallengeDone: No. Double values given or calculated in group {0} ({1} distinct values out of {2})", cellGroup, count, values.Count);
                    return Validity.Invalid;
                }
                if (result == Validity.Full && count != GridSize)
                {
                    // not all used, so not a full solution (but maybe invalid, so keep checking)
                    result = Validity.Maybe;
                }

                var avail = cellGroup.Cells.Where(c => !c.GivenOrCalculatedValue.HasValue).SelectMany(c => c.AvailableValues).Distinct().ToList();
                if (avail.Count() + count != GridSize)
                {
                    //Debug.WriteLine("IsChalllengeDone: No. Not enough values left to fill group {0}", cellGroup);
                    return Validity.Invalid;
                }
            }

            return result;
        }

        /// <summary>
        /// Tries to get a random empty position.
        /// </summary>
        /// <param name="rnd">The random.</param>
        /// <returns></returns>
        public Position GetRandomEmptyPosition(Random rnd)
        {
            for (int count = 0; count < 20; count++)
            {
                Position pos = new Position(rnd.Next(GridSize), rnd.Next(GridSize));
                Cell cell = _grid[pos.Row, pos.Column];
                if (!cell.GivenOrCalculatedValue.HasValue && cell.AvailableValues.Any())
                {
                    return pos;
                }
            }

            return null;
        }

        /// <summary>
        /// Enumerates all cells.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Cell> AllCells()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int column = 0; column < GridSize; column++)
                {
                    yield return _grid[row, column];
                }
            }
        }

        /// <summary>
        /// Gets the percentage done (cells given or calculated compared to the total number of cells).
        /// </summary>
        /// <returns></returns>
        public double GetPercentageDone()
        {
            int countDone = AllCells().Count(c => c.GivenOrCalculatedValue.HasValue);

            return (1.0 * countDone) / (GridSize * GridSize);
        }

        private CellGroup GetRowByOrdinal(int ordinal)
        {
            return GetByOrdinal(_rows, ordinal);
        }

        private CellGroup GetColumnByOrdinal(int ordinal)
        {
            return GetByOrdinal(_cols, ordinal);
        }

        /// <summary>
        /// Gets the <see cref="CellGroup"/> by it's ordinal.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        protected static CellGroup GetByOrdinal(IEnumerable<CellGroup> group, int ordinal)
        {
            return group.First(g => g.Ordinal == ordinal);
        }

        /// <summary>
        /// Clones the board, preserving size and blocks.
        /// </summary>
        /// <returns></returns>
        public abstract ISudokuGrid CloneBoardAsChallenge();

        /// <summary>
        /// Gets a value indicating whether this grid is regular.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this grid is regular; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsRegular { get; }

        /// <summary>
        /// Gets a value indicating whether this grid has special groups.
        /// </summary>
        /// <value>
        /// <c>true</c> if this grid has special groups; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasSpecialGroups { get; }

        /// <summary>
        /// Clones the grid to create a challenge. Only the given values are copied.
        /// </summary>
        /// <returns></returns>
        protected void CopyChallenge(ISudokuGrid target)
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    var sourcecell = GetCellByRowColumn(row, col);
                    if (sourcecell.GivenValue.HasValue)
                    {
                        var targetcell = target.GetCellByRowColumn(row, col);
                        targetcell.SetGivenValue(sourcecell.GivenValue.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Clears all calculated values, leaving a challenge.
        /// </summary>
        protected void ClearConclusions()
        {
            foreach (var cell in AllCells())
            {
                if (cell.GivenValue.HasValue)
                {
                    cell.AvailableValues.Clear();
                }
                else
                {
                    cell.Reset(false);
                }
            }
        }

    }
}
