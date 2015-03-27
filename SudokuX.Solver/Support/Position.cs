using System;

namespace SudokuX.Solver.Support
{
    public class Position : IEquatable<Position>
    {
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; private set; }

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
