using System.Collections.Generic;
using System.Linq;

namespace SudokuX.Solver.Support
{
    public class SelectedValue
    {
        public SelectedValue(Cell target, IEnumerable<int> remaining)
        {
            Target = target;
            Remaining = remaining.ToList();
        }

        public Cell Target { get; set; }
        public List<int> Remaining { get; private set; }

    }
}
