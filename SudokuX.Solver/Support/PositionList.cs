using System.Collections.Generic;
using System.Linq;

namespace SudokuX.Solver.Support
{
    /// <summary>
    /// A list of positions.
    /// </summary>
    public class PositionList
    {
        public PositionList()
        {
            Positions = new List<Position>();
        }

        public PositionList(IEnumerable<Position> list)
        {
            Positions = list.ToList();
        }


        public List<Position> Positions { get; private set; }

        public double SeverityScore { get; set; }
    }
}
