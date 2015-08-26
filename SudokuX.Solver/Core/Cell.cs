using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuX.Solver.Core
{
    /// <summary>
    /// A single cell in the sudoku grid.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Gets or sets the row index of this Cell.
        /// </summary>
        /// <value>
        /// The row.
        /// </value>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets the column index of this Cell.
        /// </summary>
        /// <value>
        /// The column.
        /// </value>
        public int Column { get; set; }

        private readonly int _min;
        private readonly int _max;
        private readonly List<CellGroup> _groups = new List<CellGroup>();
        private int? _givenValue;
        private int? _calculatedValue;
        private readonly List<int> _available = new List<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <exception cref="System.ArgumentException">
        /// Min value must be >= 0;min
        /// or
        /// Max value must be higher that min;max
        /// </exception>
        public Cell(int row, int column, int min, int max)
        {
            Row = row;
            Column = column;
            if (min < 0) throw new ArgumentException("Min value must be >= 0", "min");
            if (max <= min) throw new ArgumentException("Max value must be higher that min", "max");

            _min = min;
            _max = max;

            _available.AddRange(Enumerable.Range(_min, _max - _min + 1));
        }

        /// <summary>
        /// Gets or sets the name of this Cell.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the containing groups.
        /// </summary>
        /// <value>
        /// The containing groups.
        /// </value>
        public IList<CellGroup> ContainingGroups
        {
            get { return new ReadOnlyCollection<CellGroup>(_groups); }
        }

        /// <summary>
        /// Gets the available values.
        /// </summary>
        /// <value>
        /// The available values.
        /// </value>
        public IList<int> AvailableValues
        {
            get { return new ReadOnlyCollection<int>(_available); }
        }

        /// <summary>
        /// Sets the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetGivenValue(int value)
        {
            _givenValue = value;
        }

        /// <summary>
        /// Sets the calculated value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetCalculatedValue(int value)
        {
            _calculatedValue = value;
        }

        /// <summary>
        /// Gets the given value.
        /// </summary>
        /// <value>
        /// The given value.
        /// </value>
        public int? GivenValue
        {
            get { return _givenValue; }
        }

        /// <summary>
        /// Gets the calculated value.
        /// </summary>
        /// <value>
        /// The calculated value.
        /// </value>
        public int? CalculatedValue
        {
            get { return _calculatedValue; }
        }


        /// <summary>
        /// Gets a value indicating whether this instance has a given value or a calculated value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has value; otherwise, <c>false</c>.
        /// </value>
        public bool HasGivenOrCalculatedValue { get { return GivenValue.HasValue || CalculatedValue.HasValue; } }

        /// <summary>
        /// Gets or sets the complexity level used to calculate the value so far.
        /// </summary>
        /// <value>
        /// The used severity level.
        /// </value>
        public int UsedComplexityLevel { get; set; }

        private void AddToGroup(CellGroup cellGroup)
        {
            if (cellGroup == null) throw new ArgumentNullException("cellGroup");
            // this cell belongs to that group
            _groups.Add(cellGroup);
            // that group contains this cell
            cellGroup.Cells.Add(this);
        }

        /// <summary>
        /// Adds this cell to the specified group(s).
        /// </summary>
        /// <param name="groups">The groups.</param>
        public void AddToGroups(params CellGroup[] groups)
        {
            foreach (var cellGroup in groups)
            {
                AddToGroup(cellGroup);
            }
        }

        /// <summary>
        /// Erases the available value from the list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool EraseAvailable(int value)
        {
            return _available.Remove(value);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Cell " + Name;
        }

        /// <summary>
        /// Resets this cell.
        /// </summary>
        /// <param name="clearGiven">if set to <c>true</c>, also clear the given (challenge) value.</param>
        public void Reset(bool clearGiven)
        {
            if (clearGiven || !_givenValue.HasValue)
            {
                _givenValue = null;
                _calculatedValue = null;
                _available.Clear();
                _available.AddRange(Enumerable.Range(_min, _max - _min + 1));
            }
        }

        /// <summary>
        /// Is this a legal value for this cell, that is: not already used in one of it's groups?
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool GivenValueIsLegal(int val)
        {
            return _groups.SelectMany(g => g.Cells)
                .Where(c => c != this)
                .All(c => c.GivenValue != val);
        }

        /// <summary>
        /// Checks whether the supplied value is legal as given or calculated value.
        /// </summary>
        /// <param name="val">The value to check.</param>
        /// <returns><c>true</c> if legal, otherwise <c>false</c>.</returns>
        public bool GivenOrCalculatedValueIsLegal(int val)
        {
            return _groups.SelectMany(g => g.Cells)
                .Where(c => c != this)
                .All(c => c.GivenValue != val && c.CalculatedValue != val);
        }
    }
}
