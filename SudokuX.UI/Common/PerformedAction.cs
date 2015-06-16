namespace SudokuX.UI.Common
{
    /// <summary>
    /// An action to store in the undo stack: who did what on the board?
    /// </summary>
    internal class PerformedAction
    {
        public PerformedAction(Cell cell)
        {
            Cell = cell;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this action was automatic (initiated by the system, instead of the user).
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is automatic; otherwise, <c>false</c>.
        /// </value>
        public bool IsAutomatic { get; set; }

        /// <summary>
        /// Gets the cell this action is performed on.
        /// </summary>
        /// <value>
        /// The cell.
        /// </value>
        public Cell Cell { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value is set (else it is removed).
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is value set; otherwise, <c>false</c>.
        /// </value>
        public bool IsValueSet { get; set; }

        /// <summary>
        /// Gets or sets the value set or removed.
        /// </summary>
        /// <value>
        /// The int value.
        /// </value>
        public int IntValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is a "real" value, as opposed to a "pencilmark" value.
        /// </summary>
        /// <value>
        /// <c>true</c> if this is a real value; otherwise, <c>false</c>.
        /// </value>
        public bool IsRealValue { get; set; }
    }
}
