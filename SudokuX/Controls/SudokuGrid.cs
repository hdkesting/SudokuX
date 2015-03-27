using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SudokuX.Controls
{
    public partial class SudokuGrid : TableLayoutPanel
    {
        private GridField[,] _fields;
        private List<List<GridField>> _groups = new List<List<GridField>>();

        public SudokuGrid()
        {
            GridSizeX = GridSizeY = 4;
            ColumnCount = RowCount = 4;

            InitializeComponent();
        }

        public void SetBlockSize(int x, int y)
        {
            GridSizeX = x;
            GridSizeY = y;

            MakeGrid();
        }

        private int GridSizeX { get; set; }
        private int GridSizeY { get; set; }

        public int GridSize { get { return GridSizeX * GridSizeY; } }

        private void MakeGrid()
        {
            int size = 40;
            _fields = new GridField[GridSize, GridSize];
            RowStyles.Clear();
            ColumnStyles.Clear();
            RowCount = ColumnCount = GridSize;
            //GridPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, size));
            //GridPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, size));

            Width = Height = GridSize * (size + 3);

            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    var fld = new GridField(size, GridSizeY, GridSizeX);
                    if (((x / GridSizeX) + (y / GridSizeY)) % 2 == 1)
                    {
                        fld.BackColor = Color.FromArgb(220, 255, 220);
                    }
                    else
                    {
                        fld.BackColor = Color.FromArgb(220, 255, 220);
                    }

                    Controls.Add(fld, x, y);
                    _fields[x, y] = fld;
                }
            }

            BuildGroups();
        }

        private void BuildGroups()
        {
            // rijen
            for (int y = 0; y < GridSize; y++)
            {
                var grp = new List<GridField>();
                _groups.Add(grp);
                for (int x = 0; x < GridSize; x++)
                {
                    var fld = _fields[x, y];
                    grp.Add(fld);
                    fld.AddGroup(grp);
                }
            }

            // kolommen
            for (int x = 0; x < GridSize; x++)
            {
                var grp = new List<GridField>();
                _groups.Add(grp);
                for (int y = 0; y < GridSize; y++)
                {
                    var fld = _fields[x, y];
                    grp.Add(fld);
                    fld.AddGroup(grp);
                }
            }

            // blokken
            for (int b = 0; b < GridSize; b++)
            {
                int bx = (b % GridSizeX) * GridSizeX;
                int by = (b / GridSizeX) * GridSizeY;
                var grp = new List<GridField>();
                _groups.Add(grp);
                for (int x = 0; x < GridSizeX; x++)
                    for (int y = 0; y < GridSizeY; y++)
                    {
                        var fld = _fields[x + bx, y + by];
                        grp.Add(fld);
                        fld.AddGroup(grp);
                    }
            }

            // diagonalen?
        }

        public void SetValue(int x, int y, int value)
        {
            var fld = _fields[x, y];
            fld.SetValue(value, true);
            foreach (var grp in fld.ContainingGroups)
            {
                foreach (var sib in grp)
                    sib.RemovePossibility(value);
            }
        }

        public bool IsPossible(int x, int y, int value)
        {
            var fld = _fields[x, y];
            if (fld.Value.HasValue)
                return false;

            return fld.IsPossible(value);
        }

        public bool HasValue(int x, int y)
        {
            var fld = _fields[x, y];
            return fld.Value.HasValue;
        }
    }
}
