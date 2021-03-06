﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuX.Solver.Core
{
    /// <summary>
    /// A single cell in the sudoku grid.
    /// </summary>
    [Serializable]
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
        /// Gets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public int MinValue { get { return _min; } }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public int MaxValue { get { return _max; } }

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
        /// Gets the given value or calculated value for this cell, if any.
        /// </summary>
        /// <value>
        ///   <c>null</c> if the cell is still empty.
        /// </value>
        public int? GivenOrCalculatedValue { get { return GivenValue ?? CalculatedValue; } }

        /// <summary>
        /// Gets or sets the complexity level used to calculate the value so far.
        /// </summary>
        /// <value>
        /// The used severity level.
        /// </value>
        public float UsedComplexityLevel { get; set; }

        /// <summary>
        /// Gets the number of conclusions used on this cell.
        /// </summary>
        /// <value>
        /// The clues used.
        /// </value>
        public int CluesUsed { get; internal set; }

        private void AddToGroup(CellGroup cellGroup)
        {
            if (cellGroup == null) throw new ArgumentNullException("cellGroup");
            // this cell belongs to that group
            if (!_groups.Contains(cellGroup))
            {
                _groups.Add(cellGroup);
                // that group contains this cell
                cellGroup.Cells.Add(this);
            }
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
        /// Resets this cell, optionally including a given value.
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
                // DO reset the next values, all conclusions will be done again
                CluesUsed = 0;
                UsedComplexityLevel = 0;
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

        /// <summary>
        /// Prints the value.
        /// </summary>
        /// <param name="value">The 0-based internal value.</param>
        /// <returns></returns>
        public string PrintValue(int value)
        {
            return (_max < 10 ? "123456789" : "0123456789ABCDEF")[value - _min].ToString();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Row * 32 + Column;
        }
    }
}
