﻿using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;
using System;

namespace SudokuX.Solver.NextPositionStrategies
{
    /// <summary>
    /// How should I select the next position where to place a given value? 
    /// </summary>
    /// <remarks>
    /// A list of positions is turned into a number. Multiple lists are evaluated, the highest scoring one is used.
    /// </remarks>
    internal static class NextPositionStrategy
    {
        /// <summary>
        /// Counts the total number of available values in the list.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="positions">The positions.</param>
        /// <returns></returns>
        public static int TotalNumberOfAvailables(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            return positions
                .Select(p => grid.GetCellByRowColumn(p.Row, p.Column))
                .Where(c => !c.GivenOrCalculatedValue.HasValue)
                .Select(c => c.AvailableValues.Count)
                .Sum();
        }

        /// <summary>
        /// Selects the minimum count of available values from the group.
        /// </summary>
        /// <remarks>
        /// Returns a negative number for correct ordering.
        /// </remarks>
        /// <param name="grid">The grid.</param>
        /// <param name="positions">The positions.</param>
        /// <returns></returns>
        public static int MinNumberOfAvailables(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            return positions
                .Select(p => grid.GetCellByRowColumn(p.Row, p.Column))
                .Where(c => !c.GivenOrCalculatedValue.HasValue)
                .Select(c => c.AvailableValues.Count)
                .Min();
        }


        /// <summary>
        /// Selects the maximum count of available values from the group.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="positions">The positions.</param>
        /// <returns></returns>
        public static int MaxNumberOfAvailables(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            return positions
                .Select(p => grid.GetCellByRowColumn(p.Row, p.Column))
                .Where(c => !c.GivenOrCalculatedValue.HasValue)
                .Select(c => c.AvailableValues.Count)
                .Max();
        }

        /// <summary>
        /// Just selects 1, any group is just as good as any other.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="positions">The positions.</param>
        /// <returns></returns>
        public static int Any(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            return 1;
        }

        /// <summary>
        /// Selects the positions with the lowest complexity level (=least amount of conclusions applied).
        /// </summary>
        /// <remarks>
        /// Returns a negative number for correct ordering.
        /// </remarks>
        /// <param name="grid">The grid.</param>
        /// <param name="positions">The positions.</param>
        /// <returns></returns>
        public static int MinComplexityLevel(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            var v = positions
                .Select(p => grid.GetCellByRowColumn(p.Row, p.Column))
                .Where(c => !c.GivenOrCalculatedValue.HasValue)
                .Select(c => c.UsedComplexityLevel)
                .Min();
            return (int)Math.Ceiling(v);
        }

        /// <summary>
        /// Selects the positions with the lowest number of conclusions used on it.
        /// </summary>
        /// <remarks>
        /// Returns a negative number for correct ordering.
        /// </remarks>
        /// <param name="grid">The grid.</param>
        /// <param name="positions">The positions.</param>
        /// <returns></returns>
        public static int MinCluesUsed(ISudokuGrid grid, IEnumerable<Position> positions)
        {
            return positions
                .Select(p => grid.GetCellByRowColumn(p.Row, p.Column))
                .Where(c => !c.GivenOrCalculatedValue.HasValue)
                .Select(c => c.CluesUsed)
                .Min();
        }

    }
}
