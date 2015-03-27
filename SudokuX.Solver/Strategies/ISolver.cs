using System;
using System.Collections.Generic;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Strategies
{
    public interface ISolver
    {
        IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid);
    }
}
