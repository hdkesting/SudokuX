// ReSharper disable once CheckNamespace
namespace SudokuX.UI.Common.Enums
{
    /// <summary>
    /// How should a cell get a value?
    /// </summary>
    public enum ValueSelectionMode
    {
        /// <summary>
        /// No mode is selected yet.
        /// </summary>
        None,

        /// <summary>
        /// A Button Value is selected first, next are cells that should get that value.
        /// </summary>
        ButtonFirst,

        /// <summary>
        /// A Cell is selected first, next is a button to give it a value.
        /// </summary>
        CellFirst
    }

    public enum BorderType
    {
        /// <summary>
        /// The regular border, between cells in one block.
        /// </summary>
        Regular,
        /// <summary>
        /// The block border, between cells of different blocks.
        /// </summary>
        Block,
        /// <summary>
        /// The special border, beween a block in a special group and the outside.
        /// </summary>
        Special
    }

    [System.Flags]
    public enum Highlight
    {
        /// <summary>
        /// No highlight.
        /// </summary>
        None = 0,

        /// <summary>
        /// A value is found as pencil mark.
        /// </summary>
        Pencil = 1,

        /// <summary>
        /// A value is given or placed.
        /// </summary>
        Pen = 2,

        /// <summary>
        /// The recently completed group.
        /// </summary>
        Group = 4,

        /// <summary>
        /// An easily solvable field (just 1 possibility)
        /// </summary>
        Easy = 8
    }
}
