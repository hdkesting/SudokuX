using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Strategies
{
    /// <summary>
    /// Process the One Rule: have just one of each value in each group. 
    /// So remove availables when there is a given or calculated value.
    /// </summary>
    public class BasicRule : ISolver
    {
        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            Debug.WriteLine("Invoking BasicRule");
            // var valuecells = grid.AllCells().Where(c => c.HasValue).ToList();

            var result = new List<Conclusion>();

            foreach (var cell in grid.AllCells().Where(c => c.HasGivenOrCalculatedValue))
            {
                // ReSharper disable once PossibleInvalidOperationException
                int value = cell.CalculatedValue ?? cell.GivenValue.Value;

                result.AddRange(cell.ContainingGroups
                    .SelectMany(g => g.Cells)
                    .Where(c => !c.HasGivenOrCalculatedValue && c.AvailableValues.Contains(value))
                    .Select(sibling => new Conclusion(sibling, Complexity, new[] { value })));
            }

            return result;
        }

        public int Complexity
        {
            get { return 0; }
        }
    }
}
