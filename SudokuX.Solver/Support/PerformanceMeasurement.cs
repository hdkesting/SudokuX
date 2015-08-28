using System;

namespace SudokuX.Solver.Support
{
    /// <summary>
    /// A performance measurement
    /// </summary>
    public class PerformanceMeasurement
    {
        /// <summary>
        /// Gets or sets the total time spent.
        /// </summary>
        /// <value>
        /// The time spent.
        /// </value>
        public TimeSpan TimeSpent { get; set; }

        /// <summary>
        /// Gets or sets the number of invocations.
        /// </summary>
        /// <value>
        /// The invocations.
        /// </value>
        public int Invocations { get; set; }

        /// <summary>
        /// Gets or sets the total count of returned results.
        /// </summary>
        /// <value>
        /// The result count.
        /// </value>
        public int ResultCount { get; set; }
    }
}
