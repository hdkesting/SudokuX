using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Strategies
{
    /// <summary>
    /// Within a group, check for three cells with only (the same) three possibilities (or a subset): the other cells in this group can't have these three.
    /// </summary>
    public class NakedTriple : ISolver
    {
        public IEnumerable<Conclusion> ProcessGrid(ISudokuGrid grid)
        {
            Debug.WriteLine("Invoking NakedTriple");
            foreach (CellGroup group in grid.CellGroups)
            {
                var conclusions = FindNakedTriples(group).ToList();

                if (conclusions.Any())
                {
                    return conclusions;
                    // quit after the first group found something, but return the whole list for that group
                }
            }

            return Enumerable.Empty<Conclusion>();
        }

        public int Complexity
        {
            get { return 5; }
        }

        private IEnumerable<Conclusion> FindNakedTriples(CellGroup cellGroup)
        {
            var possibletriples = cellGroup.Cells.Where(c => !c.HasValue && c.AvailableValues.Count <= 3).ToList();

            // if you've processed a->b, then there's no need to process b->a

            // there's a naked triple for (123), (123), (123)
            // but also for (123), (123), (12)
            // and for (12), (23), (13)

            if (possibletriples.Count >= 3)
            {
                foreach (Cell first in possibletriples)
                {
                    var list = FindSecondAndThird(cellGroup, possibletriples, first).ToList();
                    if (list.Any())
                        return list; // quit after the first full find
                }
            }

            return Enumerable.Empty<Conclusion>();
        }


        private IEnumerable<Conclusion> FindSecondAndThird(CellGroup cellGroup, IEnumerable<Cell> candidates, Cell first)
        {
            var searchrange = candidates.ToList();
            searchrange.Remove(first);

            var firstavailables = first.AvailableValues.ToList();
            foreach (var second in searchrange)
            {
                var allavailable = firstavailables.Union(second.AvailableValues).ToList();
                if (allavailable.Count <= 3)
                {
                    var list = FindThird(cellGroup, searchrange, allavailable, first, second).ToList();
                    if (list.Any())
                    {
                        return list;
                    }
                }
            }

            return Enumerable.Empty<Conclusion>();
        }

        private IEnumerable<Conclusion> FindThird(CellGroup cellGroup, IEnumerable<Cell> candidates, IList<int> twocellavailable, Cell first, Cell second)
        {
            var searchrange = candidates.ToList();
            searchrange.Remove(second);

            foreach (var third in searchrange)
            {
                var threecellavailable = twocellavailable.Union(third.AvailableValues).ToList();
                if (threecellavailable.Count == 3)
                {
                    var list = BuildConclusions(cellGroup, first, second, third, threecellavailable).ToList();
                    if (list.Any())
                    {
                        Debug.WriteLine("Found useful naked triple for group {0}, values {1}, {2}, {3}",
                            cellGroup, threecellavailable[0], threecellavailable[1], threecellavailable[2]);
                        return list;
                    }
                }
            }

            return Enumerable.Empty<Conclusion>();
        }

        private IEnumerable<Conclusion> BuildConclusions(CellGroup cellGroup, Cell first, Cell second, Cell third,
            IList<int> triple)
        {
            foreach (var cell in cellGroup.Cells.Where(c => !c.HasValue && c != first && c != second && c != third))
            {
                // remove triplet values from cells other than that triple
                var toomuch = cell.AvailableValues.Intersect(triple).ToList();

                if (toomuch.Any())
                {
                    var c = new Conclusion(cell, Complexity, toomuch);
                    yield return c;
                }
            }
        }
    }
}
