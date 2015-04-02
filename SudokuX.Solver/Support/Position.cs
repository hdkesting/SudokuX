using System;

namespace SudokuX.Solver.Support
{
    /// <summary>
    /// A position in a grid.
    /// </summary>
    public class Position : IEquatable<Position>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        public Position(int row, int column)
        {
            if (row < 0) throw new ArgumentOutOfRangeException("row", "Row can't be negative.");
            if (column < 0) throw new ArgumentOutOfRangeException("column", "Column can't be negative.");

            Row = row;
            Column = column;
        }

        /// <summary>
        /// Gets the row index.
        /// </summary>
        /// <value>
        /// The row.
        /// </value>
        public int Row { get; private set; }

        /// <summary>
        /// Gets the column index.
        /// </summary>
        /// <value>
        /// The column.
        /// </value>
        public int Column { get; private set; }

        public bool Equals(Position other)
        {
            if (other == null) return false;

            return (other.Row == this.Row && other.Column == this.Column);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Position);
        }

        public override int GetHashCode()
        {
            return Row * 100 + Column;
        }
    }
}
