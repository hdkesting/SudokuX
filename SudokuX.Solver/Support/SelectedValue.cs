using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Core;

namespace SudokuX.Solver.Support
{
    /// <summary>
    /// A selected value.
    /// </summary>
    public class SelectedValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedValue"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="remaining">The remaining.</param>
        public SelectedValue(Cell target, IEnumerable<int> remaining)
        {
            Target = target;
            Remaining = remaining.ToList();
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public Cell Target { get; set; }

        /// <summary>
        /// Gets the list of remaining possibilities.
        /// </summary>
        /// <value>
        /// The remaining.
        /// </value>
        public List<int> Remaining { get; private set; }

    }
}
