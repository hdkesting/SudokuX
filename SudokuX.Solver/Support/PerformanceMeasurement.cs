using System;

namespace SudokuX.Solver.Support
{
    public class PerformanceMeasurement
    {
        public TimeSpan TimeSpent { get; set; }

        public int Invocations { get; set; }

        public int ResultCount { get; set; }
    }
}
