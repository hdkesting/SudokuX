using System;
using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Support
{
    /// <summary>
    /// A strategy conclusion about a particular cell.
    /// </summary>
    public class Conclusion : IEquatable<Conclusion>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Conclusion"/> class for a calculated value.
        /// </summary>
        /// <param name="targetCell">The target cell.</param>
        /// <param name="complexityLevel">The complexity level.</param>
        private Conclusion(SolverType solverType, Cell targetCell, int complexityLevel, IEnumerable<Cell> reasonCells)
        {
            SolverType = solverType;
            TargetCell = targetCell;
            ComplexityLevel = complexityLevel;
            ExcludedValues = new List<int>();
            ReasonCells = new System.Collections.ObjectModel.ReadOnlyCollection<Cell>(reasonCells.ToList());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Conclusion" /> class for excluded values.
        /// </summary>
        /// <param name="solverType">Type of the solver.</param>
        /// <param name="targetCell">The target cell.</param>
        /// <param name="complexityLevel">The complexity level.</param>
        /// <param name="excludedValues">The excluded values.</param>
        /// <param name="reasonCells">The reason for this conclusion.</param>
        public Conclusion(SolverType solverType, Cell targetCell, int complexityLevel, IEnumerable<int> excludedValues, IEnumerable<Cell> reasonCells)
            : this(solverType, targetCell, complexityLevel, reasonCells)
        {
            ExcludedValues.AddRange(excludedValues);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Conclusion" /> class for a calculated value.
        /// </summary>
        /// <param name="solverType">Type of the solver.</param>
        /// <param name="targetCell">The target cell.</param>
        /// <param name="complexityLevel">The complexity level.</param>
        /// <param name="exactValue">The exact value.</param>
        /// <param name="reasonCells">The reason for this conclusion.</param>
        public Conclusion(SolverType solverType, Cell targetCell, int complexityLevel, int exactValue, IEnumerable<Cell> reasonCells)
            : this(solverType, targetCell, complexityLevel, reasonCells)
        {
            ExactValue = exactValue;
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
        /// Gets the cells that are the reason for this <see cref="Conclusion"/>.
        /// </summary>
        /// <value>
        /// The reason cells.
        /// </value>
        public System.Collections.ObjectModel.ReadOnlyCollection<Cell> ReasonCells { get; private set; }

        public Enums.SolverType SolverType { get; set; }

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

        public override bool Equals(object obj)
        {
            var other = obj as Conclusion;
            return other == null ? false : Equals(other);
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
                + (ExactValue ?? 21) * 128
                + ExcludedValues.Count * 2048;
        }

        public override string ToString()
        {
            return TargetCell.ToString() +
                (ExactValue.HasValue
                    ? " = " + ExactValue
                    : " -- " + ExcludedValues.Aggregate("", (s, ev) => s + TargetCell.PrintValue(ev) + ", "));
        }
    }
}