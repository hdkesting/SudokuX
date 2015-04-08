using System;

namespace SudokuX.Solver.Support
{
    public class ProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the total number of cells.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the number of given values in the challenge.
        /// </summary>
        /// <value>
        /// The given.
        /// </value>
        public int Given { get; set; }

        /// <summary>
        /// Gets or sets the number of calculated values.
        /// </summary>
        /// <value>
        /// The calculated.
        /// </value>
        public int Calculated { get; set; }

        /// <summary>
        /// Gets the number of given or calculated values.
        /// </summary>
        /// <value>
        /// The given or calculated.
        /// </value>
        public int GivenOrCalculated
        {
            get { return Given + Calculated; }
        }

        /// <summary>
        /// Gets the percentage done (0..100).
        /// </summary>
        /// <value>
        /// The percentage done.
        /// </value>
        public int PercentageDone
        {
            get { return Total == 0 ? 0 : (GivenOrCalculated * 100) / Total; }
        }
    }
}
