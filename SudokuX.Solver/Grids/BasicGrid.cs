using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public int GridSize { get; private set; }

        /// <summary>
        /// Gets the inclusive minimum value of a grid cell.
        /// </summary>
        public int MinValue { get; private set; }

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
            return AllCells().All(c => c.HasValue);
        }

        /// <summary>
        /// Is this a valid full solution?
        /// </summary>
        /// <returns></returns>
        public Validity IsChallengeDone()
        {
            Validity result = Validity.Full;
            if (AllCells().Any(c => !c.HasValue && !c.AvailableValues.Any()))
            {
                Debug.WriteLine("IsChallengeDone: No. No availables left in some empty cell.");
                return Validity.Invalid;
            }

            foreach (var cellGroup in CellGroups)
            {
                var values = cellGroup.Cells.Select(c => c.GivenValue ?? c.CalculatedValue).Where(c => c.HasValue).ToList();
                int count = values.Distinct().Count();
                if (count != values.Count)
                {
                    Debug.WriteLine("IsChallengeDone: No. Double values given or calculated in group {0} ({1} distinct values out of {2})", cellGroup, count, values.Count);
                    return Validity.Invalid;
                }
                if (result == Validity.Full && count != GridSize)
                {
                    // not all used, so not a full solution (but maybe invalid, so keep checking)
                    result = Validity.Maybe;
                }

                var avail = cellGroup.Cells.Where(c => !c.HasValue).SelectMany(c => c.AvailableValues).Distinct().ToList();
                if (avail.Count() + count != GridSize)
                {
                    Debug.WriteLine("IsChalllengeDone: No. Not enough values left to fill group {0}", cellGroup);
                    return Validity.Invalid;
                }
            }

            return result;
        }

        public Position GetRandomEmptyPosition(Random rnd)
        {
            for (int count = 0; count < 20; count++)
            {
                Position pos = new Position(rnd.Next(GridSize), rnd.Next(GridSize));
                Cell cell = _grid[pos.Row, pos.Column];
                if (!cell.GivenValue.HasValue && cell.AvailableValues.Any())
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

        public double GetPercentageDone()
        {
            int countDone = AllCells().Count(c => c.HasValue);

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

        protected static CellGroup GetByOrdinal(IEnumerable<CellGroup> group, int ordinal)
        {
            return group.First(g => g.Ordinal == ordinal);
        }

    }
}
