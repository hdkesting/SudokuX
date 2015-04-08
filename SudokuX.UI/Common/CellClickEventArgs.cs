using System;

namespace SudokuX.UI.Common
{
    public class CellClickEventArgs : EventArgs
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public CellClickEventArgs(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
