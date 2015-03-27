using System;

namespace SudokuX.Solver.Support
{
    public class ProgressEventArgs : EventArgs
    {
        public int Total { get; set; }

        public int Given { get; set; }

        public int Calculated { get; set; }

        public int GivenOrCalculated
        {
            get { return Given + Calculated; }
        }

        public int PercentageDone
        {
            get { return Total == 0 ? 0 : (GivenOrCalculated * 100) / Total; }
        }
    }
}
