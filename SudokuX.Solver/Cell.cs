using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuX.Solver
{
    public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        private readonly int _min;
        private readonly int _max;
        private readonly List<CellGroup> _groups = new List<CellGroup>();
        private int? _givenValue;
        private int? _calculatedValue;
        private readonly List<int> _available = new List<int>();

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

        public string Name { get; set; }

        public IList<CellGroup> ContainingGroups
        {
            get { return new ReadOnlyCollection<CellGroup>(_groups); }
        }

        public IList<int> AvailableValues
        {
            get { return new ReadOnlyCollection<int>(_available); }
        }

        public void SetGivenValue(int value)
        {
            _givenValue = value;
            //EraseAvailableFromGroups(value);
        }

        public void SetCalculatedValue(int value)
        {
            _calculatedValue = value;
            //EraseAvailableFromGroups(value);
        }

        public int? GivenValue
        {
            get { return _givenValue; }
        }

        public int? CalculatedValue
        {
            get { return _calculatedValue; }
        }


        /// <summary>
        /// Gets a value indicating whether this instance has given or calculated value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has value; otherwise, <c>false</c>.
        /// </value>
        public bool HasValue { get { return GivenValue.HasValue || CalculatedValue.HasValue; } }

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

        public void AddToGroups(params CellGroup[] groups)
        {
            foreach (var cellGroup in groups)
            {
                AddToGroup(cellGroup);
            }
        }

        private bool EraseAvailableFromGroups(int value)
        {
            var done = false;
            foreach (var cellGroup in _groups)
            {
                done |= cellGroup.EraseAvailable(value);
            }
            return done;
        }

        public bool EraseAvailable(int value)
        {
            return _available.Remove(value);
        }

        public override string ToString()
        {
            return "Cell " + Name;
        }

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

        public bool GivenOrCalculatedValueIsLegal(int val)
        {
            return _groups.SelectMany(g => g.Cells)
                .Where(c => c != this)
                .All(c => c.GivenValue != val && c.CalculatedValue != val);
        }
    }
}
