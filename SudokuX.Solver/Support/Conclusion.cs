using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuX.Solver.Support
{
    /// <summary>
    /// A strategy conclusion about a particular cell.
    /// </summary>
    public class Conclusion : IEquatable<Conclusion>
    {
        public Conclusion(Cell targetCell, int complexityLevel)
        {
            TargetCell = targetCell;
            ComplexityLevel = complexityLevel;
            ExcludedValues = new List<int>();
        }

        public Conclusion(Cell targetCell, int complexityLevel, IEnumerable<int> excludedValues)
            : this(targetCell, complexityLevel)
        {
            ExcludedValues.AddRange(excludedValues);
        }

        /// <summary>
        /// Gets the target cell this conclusion is about.
        /// </summary>
        /// <value>
        /// The target cell.
        /// </value>
        public Cell TargetCell { get; private set; }

        /// <summary>
        /// Gets or sets the calculated exact value of the <see cref="TargetCell"/>.
        /// </summary>
        /// <value>
        /// The exact value.
        /// </value>
        public int? ExactValue { get; set; }

        /// <summary>
        /// Gets the calculated excluded values for the <see cref="TargetCell"/>.
        /// </summary>
        /// <value>
        /// The excluded values.
        /// </value>
        public List<int> ExcludedValues { get; private set; }

        /// <summary>
        /// Gets the complexity level used to arrive at this conclusion.
        /// </summary>
        /// <value>
        /// The complexity level.
        /// </value>
        public int ComplexityLevel { get; private set; }

        private static bool ListsAreEqual(IList<int> input, IList<int> check)
        {
            if (input.Count != check.Count)
                return false;

            return input.All(check.Contains);
        }

        public bool Equals(Conclusion other)
        {
            return TargetCell == other.TargetCell &&
                ExactValue == other.ExactValue &&
                ListsAreEqual(ExcludedValues, other.ExcludedValues);
        }

        public override int GetHashCode()
        {
            return TargetCell.GetHashCode()
                + (ExactValue ?? 21) * 13
                + ExcludedValues.Sum() * 17;
        }
    }
}