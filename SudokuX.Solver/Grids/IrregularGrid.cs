using System;
using System.Collections.Generic;
using System.Linq;
using SudokuX.Solver.Support;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Base class for irregular-blocked grids.
    /// </summary>
    public abstract class IrregularGrid : BasicGrid
    {
        private readonly List<CellGroup> _blocks = new List<CellGroup>();

        private readonly Random _rng = new Random();

        private readonly int[,] _blockgrid;


        protected IrregularGrid(int startblockwidth, int startblockheight)
            : base(startblockwidth * startblockheight)
        {
            _blockgrid = new int[GridSize, GridSize];
            InitializeBlockGrid(startblockwidth, startblockheight);
            Permute();

            AssignBlocks();
        }

        private void AssignBlocks()
        {
            // create blockgroups
            for (int i = 0; i < GridSize; i++)
            {
                _blocks.Add(new CellGroup(GridSize, i) { Name = "Block " + i, GroupType = GroupType.Block });
            }

            // add cells to blocks
            for (int r = 0; r < GridSize; r++)
            {
                for (int c = 0; c < GridSize; c++)
                {
                    var blocknr = _blockgrid[r, c];
                    var block = GetByOrdinal(_blocks, blocknr);

                    var cell = GetCellByRowColumn(r, c);
                    cell.Name = String.Format("r {0}, c {1}, b {2}", r, c, block.Ordinal);

                    cell.AddToGroups(block);
                }
            }
        }


        /// <summary>
        /// Initializes the block grid with a basic square grid.
        /// </summary>
        /// <param name="startblockwidth">The startblockwidth.</param>
        /// <param name="startblockheight">The startblockheight.</param>
        private void InitializeBlockGrid(int startblockwidth, int startblockheight)
        {
            for (int r = 0; r < GridSize; r++)
            {
                for (int c = 0; c < GridSize; c++)
                {
                    var blockrow = r / startblockheight;
                    var blockcol = c / startblockwidth;

                    _blockgrid[r, c] = blockrow * startblockheight + blockcol;
                }
            }
        }

        /// <summary>
        /// Performs a number of block switches to scramble the grid.
        /// </summary>
        private void Permute()
        {
            bool ok = true;
            for (int i = 0; i < GridSize * GridSize * 3; i++)
            {
                ok = ok && PermuteOnce();
            }
        }

        /// <summary>
        /// Performs one switch of two blockvalues.
        /// </summary>
        private bool PermuteOnce()
        {
            int counter = 0;
            while (counter < 1000)
            {
                counter++;

                Position source = null;
                Position opposite = null;
                while (opposite == null)
                {
                    // get a random position
                    int r = _rng.Next(GridSize);
                    int c = _rng.Next(GridSize);
                    source = new Position(r, c);
                    // try and get a neighbour in a different block
                    opposite = GetDifferentNeighbour(source);
                }

                var orgvalue = _blockgrid[source.Row, source.Column];
                var buurvalue = _blockgrid[opposite.Row, opposite.Column];

                // try and get a different pair of positions with the same blocknumbers
                var other = GetSimilarPairs(buurvalue, orgvalue)
                    .Where(t => t.Item1.Row != opposite.Row || t.Item1.Column != opposite.Column)
                    .Where(t => t.Item2.Row != source.Row || t.Item2.Column != source.Column)
                    .GetRandom(_rng);

                // did I find one?
                if (other != null)
                {
                    // create a copy to try out the switch
                    var copy = (int[,])_blockgrid.Clone();

                    // switch two block values
                    var val = copy[source.Row, source.Column];
                    copy[source.Row, source.Column] = copy[other.Item1.Row, other.Item1.Column];
                    copy[other.Item1.Row, other.Item1.Column] = val;

                    // are all blocks still connected?
                    if (AllBlocksConnected(copy))
                    {
                        // yes, so repeat switch on real grid - done
                        val = _blockgrid[source.Row, source.Column];
                        _blockgrid[source.Row, source.Column] = _blockgrid[other.Item1.Row, other.Item1.Column];
                        _blockgrid[other.Item1.Row, other.Item1.Column] = val;

                        return true;
                    }

                    // nope, a disconnected block. 
                }

                // Keep trying
            }

            return false;
        }

        /// <summary>
        /// Are all the blocks connected?
        /// </summary>
        /// <param name="grid">The block-grid to check (maybe a copy of the real one).</param>
        /// <returns></returns>
        private bool AllBlocksConnected(int[,] grid)
        {
            // check every block
            for (int blocknr = 0; blocknr < GridSize; blocknr++)
            {
                // get some cell in that block
                var start = GetAllPositions().First(p => grid[p.Row, p.Column] == blocknr);

                var cellist = new List<Position>();
                // collect all connected cells of that block
                AddBlockCells(grid, cellist, start, blocknr);
                // did we find the whole block?
                if (cellist.Count != GridSize)
                    return false;
            }

            // everything passed!
            return true;
        }

        /// <summary>
        /// Recursively adds the other cells of the block to the list.
        /// </summary>
        /// <param name="grid">The grid to read.</param>
        /// <param name="cellist">The cellist.</param>
        /// <param name="start">The start.</param>
        /// <param name="blocknr">The blocknr.</param>
        private void AddBlockCells(int[,] grid, List<Position> cellist, Position start, int blocknr)
        {
            if (!cellist.Contains(start))
            {
                cellist.Add(start);

                foreach (var buur in GetNeighbours(start).Where(p => grid[p.Row, p.Column] == blocknr))
                {
                    AddBlockCells(grid, cellist, buur, blocknr);
                }
            }
        }

        /// <summary>
        /// Gets pairs of neighbouring cells with the given block values.
        /// </summary>
        /// <param name="firstvalue">The firstvalue.</param>
        /// <param name="secondvalue">The secondvalue.</param>
        /// <returns></returns>
        IEnumerable<Tuple<Position, Position>> GetSimilarPairs(int firstvalue, int secondvalue)
        {
            return from p1 in GetAllPositions().Where(p => _blockgrid[p.Row, p.Column] == firstvalue)
                   from p2 in GetNeighbours(p1).Where(p => _blockgrid[p.Row, p.Column] == secondvalue)
                   select Tuple.Create(p1, p2);
        }

        /// <summary>
        /// Gets the first neighbour with a different block value (if any).
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <returns></returns>
        Position GetDifferentNeighbour(Position pos)
        {
            var orgval = Grid[pos.Row, pos.Column];

            return GetNeighbours(pos)
                .Where(p => Grid[p.Row, p.Column] != orgval)
                .GetRandom(_rng);
        }

        /// <summary>
        /// Gets all positions in the grid.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Position> GetAllPositions()
        {
            for (int r = 0; r < GridSize; r++)
                for (int c = 0; c < GridSize; c++)
                    yield return new Position(r, c);
        }

        /// <summary>
        /// Gets the neighbours of a position.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <returns></returns>
        private IEnumerable<Position> GetNeighbours(Position pos)
        {
            if (pos.Row > 0)
            {
                yield return new Position(pos.Row - 1, pos.Column);
            }

            if (pos.Column > 0)
            {
                yield return new Position(pos.Row, pos.Column - 1);
            }

            if (pos.Row + 1 < GridSize)
            {
                yield return new Position(pos.Row + 1, pos.Column);
            }

            if (pos.Column + 1 < GridSize)
            {
                yield return new Position(pos.Row, pos.Column + 1);
            }
        }

        /// <summary>
        /// Gets the cell groups.
        /// </summary>
        /// <value>
        /// The cell groups.
        /// </value>
        public override IEnumerable<CellGroup> CellGroups
        {
            get
            {
                foreach (var cellGroup in base.CellGroups)
                {
                    yield return cellGroup;
                }

                foreach (var cellGroup in _blocks)
                {
                    yield return cellGroup;
                }
            }
        }

    }
}
