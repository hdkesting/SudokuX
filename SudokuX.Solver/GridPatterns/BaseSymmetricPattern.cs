using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.GridPatterns
{
    [Obsolete]
    public abstract class BaseSymmetricPattern //: IGridPattern
    {
        private readonly ISudokuGrid _grid;
        private readonly Random _rng;
        private readonly Stack<Position> _next = new Stack<Position>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSymmetricPattern"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="rng">The RNG.</param>
        /// <exception cref="System.ArgumentNullException">grid</exception>
        protected BaseSymmetricPattern(ISudokuGrid grid, Random rng = null)
        {
            if (grid == null) throw new ArgumentNullException("grid");
            _grid = grid;
            Size = grid.GridSize;
            _rng = rng ?? new Random();
        }

        protected int Size { get; private set; }

        public Position Next()
        {
            // is there a position to keep symmetry?
            Position p = Remaining().FirstOrDefault();
            if (p != null)
            {
                Debug.WriteLine("Next: Position for symmetry: {0},{1}", p.Row, p.Column);
                return p;
            }

            //if (AllDone())
            //    return null;

            var next = GetBestNextPositions();
            p = next.Positions.First();
            foreach (var pos in next.Positions.Skip(1).Distinct())
            {
                _next.Push(pos);
            }

            Debug.WriteLine("Next: new position: {0},{1} ({2} extra)", p.Row, p.Column, _next.Count);
            return p;
        }

        private PositionList GetBestNextPositions()
        {
            var sample = new List<PositionList>();
            for (int i = 0; i < Size; i++)
            {
                sample.Add(GetRandomNextPositions());
            }

            // get the lowest scored group
            return sample.OrderByDescending(s => s.SeverityScore).First();
        }

        private PositionList GetRandomNextPositions()
        {
            Position p;
            do
            {
                p = new Position(_rng.Next(Size), _rng.Next(Size));
            } while (IsDone(p));

            var list = new PositionList();
            list.Positions.Add(p);
            list.SeverityScore += GetCellComplexityScore(p);

            // remember the symmetric positions
            foreach (var symmetricPosition in GetSymmetricPositions(p.Row, p.Column))
            {
                list.Positions.Add(symmetricPosition);
                list.SeverityScore += GetCellComplexityScore(symmetricPosition);
            }

            return list;
        }

        private int GetCellComplexityScore(Position pos)
        {
            var cell = _grid.GetCellByRowColumn(pos.Row, pos.Column);
            if (cell.HasValue)
                return 0;
            return cell.AvailableValues.Count;
        }

        public IEnumerable<Position> Remaining()
        {
            Position p;
            while ((p = _next.TryPop()) != null)
            {
                if (!IsDone(p))
                {
                    yield return p;
                }
            }
        }

        public void Reset()
        {
            _next.Clear();

            // reset
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    if (IsDone(new Position(r, c)))
                    {
                        foreach (var position in GetSymmetricPositions(r, c))
                        {
                            if (!IsDone(position))
                            {
                                _next.Push(position);
                            }
                        }
                    }
                }
            }
        }

        protected abstract IEnumerable<Position> GetSymmetricPositions(int row, int column);

        /// <summary>
        /// Does this cell contain a given value?
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool IsDone(Position position)
        {
            var cell = _grid.GetCellByRowColumn(position.Row, position.Column);
            return cell.GivenValue.HasValue;
        }

        ///// <summary>
        ///// Is this cell a good candidate for selecting a value?
        ///// </summary>
        ///// <param name="position"></param>
        ///// <returns></returns>
        //private bool IsGoodCandidate(Position position)
        //{
        //    var cell = _grid.GetCellByRowColumn(position.Row, position.Column);
        //    return !cell.HasValue;
        //}

        //private bool AllDone() test of ALLES ingevuld is, hoort niet te gebeuren
        //{
        //    for (int r = 0; r < Size; r++)
        //    {
        //        for (int c = 0; c < Size; c++)
        //        {
        //            if (!IsDone(new Position(r, c)))
        //                return false;
        //        }
        //    }

        //    return true;
        //}

    }
}
