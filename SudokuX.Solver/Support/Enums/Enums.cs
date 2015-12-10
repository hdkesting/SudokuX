namespace SudokuX.Solver.Support.Enums
{
    /// <summary>
    /// The size of the board.
    /// </summary>
    public enum BoardSize
    {
        /// <summary>
        /// A 4x4 board of 2x2 blocks
        /// </summary>
        Board4,

        /// <summary>
        /// A 6x6 board of 3x2 blocks
        /// </summary>
        Board6,

        /// <summary>
        /// A 9x9 board of 3x3 blocks
        /// </summary>
        Board9,

        /// <summary>
        /// A 9x9 board of 3x3 blocks with 2 diagonals
        /// </summary>
        Board9X,

        /// <summary>
        /// A 12x12 board of 4x3 blocks
        /// </summary>
        Board12,

        /// <summary>
        /// A 16x16 board of 4x4 blocks
        /// </summary>
        Board16,

        /// <summary>
        /// An irregular 9x9 board
        /// </summary>
        Irregular9,

        /// <summary>
        /// An irregular 6x6 board
        /// </summary>
        Irregular6,

        /// <summary>
        /// A 9x9 board with 4 extra blocks
        /// </summary>
        Hyper9,

        /// <summary>
        /// An irregular 12x12 grid. DOES NOT WORK.
        /// </summary>
        Irregular12,

        /// <summary>
        /// An 8x8 board of 4x2 blocks
        /// </summary>
        Board8Row,

        /// <summary>
        /// An 8x8 board of 2x4 blocks
        /// </summary>
        Board8Column,

        /// <summary>
        /// An 8x8 board with a mix of horizontal and vertical 2x4 blocks.
        /// </summary>
        Board8Mix
    }

    /// <summary>
    /// The validity of a calculated sudoku challenge.
    /// </summary>
    public enum Validity
    {
        /// <summary>
        /// The challenge is invalid.
        /// </summary>
        Invalid,

        /// <summary>
        /// The challenge may be valid, but needs extra clues.
        /// </summary>
        Maybe,

        /// <summary>
        /// The challenge is fully valid. This can be solved.
        /// </summary>
        Full
    }

    /// <summary>
    /// The type of cell-group.
    /// </summary>
    public enum GroupType
    {
        /// <summary>
        /// The type of this group is unknown (yet?). 
        /// </summary>
        Unknown,

        /// <summary>
        /// This group is a row.
        /// </summary>
        Row,

        /// <summary>
        /// This group is a column.
        /// </summary>
        Column,

        /// <summary>
        /// This group is a block.
        /// </summary>
        Block,

        /// <summary>
        /// This is a special group like a diagonal.
        /// </summary>
        SpecialLine,

        /// <summary>
        /// This is a special group like a hyper block.
        /// </summary>
        SpecialBlock
    }

    /// <summary>
    /// Difficulty level of a sudoku challenge
    /// </summary>
    public enum Difficulty
    {
        /// <summary>
        /// Easy
        /// </summary>
        Easy,

        /// <summary>
        /// Normal, the default level
        /// </summary>
        Normal,

        /// <summary>
        /// Harder
        /// </summary>
        Harder
    }

    /// <summary>
    /// The type of the solver
    /// </summary>
    public enum SolverType
    {
        /// <summary>
        /// An unknown solver
        /// </summary>
        Unknown,

        /// <summary>
        /// The basic rule
        /// </summary>
        Basic,

        /// <summary>
        /// The naked single strategy
        /// </summary>
        NakedSingle,

        /// <summary>
        /// The hidden single strategy
        /// </summary>
        HiddenSingle,

        /// <summary>
        /// The naked double strategy
        /// </summary>
        NakedDouble,

        /// <summary>
        /// The hidden double strategy
        /// </summary>
        HiddenDouble,

        /// <summary>
        /// The naked triple strategy
        /// </summary>
        NakedTriple,

        /// <summary>
        /// The hidden triple strategy
        /// </summary>
        HiddenTriple,

        /// <summary>
        /// The naked quad strategy
        /// </summary>
        NakedQuad,

        /// <summary>
        /// The hidden quad strategy
        /// </summary>
        HiddenQuad,

        /// <summary>
        /// The locked candidates strategy
        /// </summary>
        LockedCandidates,

        /// <summary>
        /// Solving with colors strategy
        /// </summary>
        SolveWithColors,

        /// <summary>
        /// The X-wing strategy
        /// </summary>
        XWing,

        /// <summary>
        /// The XY-wing strategy
        /// </summary>
        XYWing
    }
}
