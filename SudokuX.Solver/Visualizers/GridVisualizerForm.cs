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
                GridLabel.Text = _sudokuGrid.PrintGrid(cell => cell.GivenOrCalculatedValue.HasValue ? "x " : cell.UsedComplexityLevel.ToString() + " ");
            }
            else if (sender == radioClues)
            {
                GridLabel.Text = _sudokuGrid.PrintGrid(cell => cell.GivenOrCalculatedValue.HasValue ? "x " : cell.CluesUsed.ToString() + " ");
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
