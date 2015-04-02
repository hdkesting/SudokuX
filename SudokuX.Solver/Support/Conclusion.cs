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

        /// <summary>
        /// Returns a value indicating whether the to lists contain the same values, not necessarily in the same order.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="check">The check.</param>
        /// <returns></returns>
        private static bool ListsAreEqual(IList<int> input, IList<int> check)
        {
            if (input.Count != check.Count)
                return false;

            return input.All(check.Contains);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Conclusion other)
        {
            return TargetCell == other.TargetCell &&
                ExactValue == other.ExactValue &&
                ListsAreEqual(ExcludedValues, other.ExcludedValues);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return TargetCell.GetHashCode()
                + (ExactValue ?? 21) * 13
                + ExcludedValues.Sum() * 17;
        }
    }
}