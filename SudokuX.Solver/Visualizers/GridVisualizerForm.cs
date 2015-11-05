using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuX.Solver.Visualizers
{
    internal partial class GridVisualizerForm : Form
    {
        private Grids.BasicGrid _sudokuGrid;
        public GridVisualizerForm(Grids.BasicGrid sudokuGrid)
        {
            InitializeComponent();

            _sudokuGrid = sudokuGrid;
            this.Text = _sudokuGrid.GetType().Name; // form title

            //GridLabel.Text = _sudokuGrid.ToString();
            radioSolution.Checked = true;
            Radio_Click(radioSolution, null);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            base.OnKeyDown(e);
        }

        private void Radio_Click(object sender, EventArgs e)
        {
            if (sender == radioSolution)
            {
                GridLabel.Text = _sudokuGrid.ToString();
            }
            else if (sender == radioChallenge)
            {
                GridLabel.Text = _sudokuGrid.ToChallengeString();
            }
            else if (sender == radioComplexity)
            {
                int max = Math.Max(1, _sudokuGrid.AllCells().Select(c => c.UsedComplexityLevel).Max().ToString().Length);

                GridLabel.Text = _sudokuGrid.PrintGrid(max + 1, cell => cell.GivenOrCalculatedValue.HasValue ? "x" : cell.UsedComplexityLevel.ToString());
            }
            else if (sender == radioClues)
            {
                int max = Math.Max(1, _sudokuGrid.AllCells().Select(c => c.CluesUsed).Max().ToString().Length);
                GridLabel.Text = _sudokuGrid.PrintGrid(max + 1, cell => cell.GivenOrCalculatedValue.HasValue ? "x" : cell.CluesUsed.ToString());
            }
            else if (sender == radioAvailable)
            {
                int max = _sudokuGrid.GridSize;

                GridLabel.Text = _sudokuGrid.PrintGrid(max + 1,
                    cell => {
                        if (cell.GivenValue.HasValue)
                            return "[" + cell.PrintValue(cell.GivenValue.Value) + "]";
                        if (cell.CalculatedValue.HasValue)
                            return "(" + cell.PrintValue(cell.CalculatedValue.Value) + ")";

                        return Enumerable.Range(cell.MinValue, cell.MaxValue-cell.MinValue+1)
                            .Select(v => cell.AvailableValues.Contains(v) ? cell.PrintValue(v) : ".")
                            .Aggregate("", (s, av) => s + av);
                    });
            }
        }

        private void Radio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
