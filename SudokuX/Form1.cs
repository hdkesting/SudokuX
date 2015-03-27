using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SudokuX.Controls;
using SudokuX.Solver;

namespace SudokuX
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer _timer;

        Random _rnd = new Random();

        public Form1()
        {
            InitializeComponent();

            sudokuGrid1.SetBlockSize(3, 3);

            // 4: margin/padding per veld; 13: margin tussen gridpanel en schermrand; 20: hoogte titlebar (gok)
            Width = sudokuGrid1.Location.X + sudokuGrid1.Width + 13;
            Height = sudokuGrid1.Location.Y + sudokuGrid1.Height + 13 + 20;


            _timer = new Timer();
            _timer.Interval = 2000;
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        void TimerTick(object sender, EventArgs e)
        {
            _timer.Stop();

            var grid = Solver.ChallengeCreator.Create9X9WithX();

            foreach (var cell in grid.AllCells().Where(c => c.GivenValue.HasValue))
            {
                int r = cell.Row;
                int c = cell.Column;
                sudokuGrid1.SetValue(r, c, cell.GivenValue.Value-1);

            }
            //int x = _rnd.Next(sudokuGrid1.GridSize);
            //int y = _rnd.Next(sudokuGrid1.GridSize);

            //if (!sudokuGrid1.HasValue(x, y))
            //{
            //    bool done = false;
            //    do
            //    {
            //        int v = _rnd.Next(sudokuGrid1.GridSize);
            //        if (sudokuGrid1.IsPossible(x, y, v))
            //        {
            //            sudokuGrid1.SetValue(x, y, v);
            //            done = true;
            //        }

            //    } while (!done);
            //}
        }
    }
}
