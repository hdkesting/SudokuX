using System.Collections.Generic;
using System.Linq;

namespace SudokuX.Solver.Support
{
    /// <summary>
    /// A list of positions.
    /// </summary>
    public class PositionList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PositionList"/> class.
        /// </summary>
        public PositionList()
        {
            Positions = new List<Position>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionList"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public PositionList(IEnumerable<Position> list)
        {
            Positions = list.ToList();
        }


        /// <summary>
        /// Gets the positions.
        /// </summary>
        /// <value>
        /// The positions.
        /// </value>
        public List<Position> Positions { get; private set; }

        /// <summary>
        /// Gets or sets the severity score.
        /// </summary>
        /// <value>
        /// The severity score.
        /// </value>
        public double SeverityScore { get; set; }
    }
}
