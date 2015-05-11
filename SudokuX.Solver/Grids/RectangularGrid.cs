using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SudokuX.Solver.Strategies;
using SudokuX.Solver.Support.Enums;

namespace SudokuX.Solver.Grids
{
    /// <summary>
    /// Base class for grids with rectangular (or even square) blocks.
    /// </summary>
    public abstract class RectangularGrid : BasicGrid, IRegularSudokuGrid
    {
        private readonly List<CellGroup> _blocks = new List<CellGroup>();

        public int BlockWidth { get; private set; }
        public int BlockHeight { get; private set; }

        protected RectangularGrid(int blockwidth, int blockheight)
            : base(blockwidth * blockheight)
        {
            BlockWidth = blockwidth;
            BlockHeight = blockheight;

            Initialize();
        }

        private void Initialize()
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
                    var block = GetBlockByRowColumn(r, c);

                    var cell = GetCellByRowColumn(r, c);
                    cell.Name = String.Format("r {0}, c {1}, b {2}", r, c, block.Ordinal);

                    cell.AddToGroups(block);
                }
            }
        }

        /// <summary>
        /// Gets all defined groups of cells.
        /// </summary>
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


        //private CellGroup GetRowByOrdinal(int ordinal)
        //{
        //    return GetByOrdinal(_rows, ordinal);
        //}

        //private CellGroup GetColumnByOrdinal(int ordinal)
        //{
        //    return GetByOrdinal(_cols, ordinal);
        //}

        private CellGroup GetBlockByRowColumn(int row, int column)
        {
            return GetByOrdinal(_blocks, GetBlockOrdinalByRowColumn(row, column));
        }

        private int GetBlockOrdinalByRowColumn(int row, int column)
        {
            var blockrow = row / BlockHeight;
            var blockcol = column / BlockWidth;

            return blockrow * BlockHeight + blockcol;
        }

        protected void InitializeFromString(string challenge)
        {
            // input: a string like the ToChallengeString output
            /*
              *   +--+--+
              *   |1.|34|
              *   |34|1.|
              *   +--+--+
              *   |.3|41|
              *   |41|.3|
              *   +--+--+
              */
            // There may be empty lines above and below, or leading/trailing whitespace.
            // Maybe no block delimiters.

            const string chars = "0123456789ABCDEF";

            using (var sr = new StringReader(challenge))
            {
                string line;
                int row = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Replace('+', ' ').Replace('-', ' ').Replace('*', ' ').Replace('|', ' ');
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        line = line.Trim().Replace("|", "").Replace(" ", "").ToUpperInvariant();
                        if (line.Length != GridSize)
                            throw new InvalidOperationException(String.Format("Linesize ({0}) does not conform to gridsize ({1})", line.Length, GridSize));
                        if (row >= GridSize)
                            throw new InvalidOperationException("Too many lines");

                        for (int c = 0; c < line.Length; c++)
                        {
                            int pos = chars.IndexOf(line[c]);
                            if (pos >= 0)
                            {
                                var cell = GetCellByRowColumn(row, c);
                                cell.SetGivenValue(pos);
                            }
                        }

                        row++;
                    }
                }
            }

            // post-processing: set availables
            var rule = new BasicRule();
            var conclusions = rule.ProcessGrid(this);
            foreach (var conclusion in conclusions)
            {
                var cell = conclusion.TargetCell;
                foreach (var excludedValue in conclusion.ExcludedValues)
                {
                    cell.EraseAvailable(excludedValue);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the calculated solution of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            /*
             *   +--+--+
             *   |12|34|
             *   |34|12|
             *   +--+--+
             *   |23|41|
             *   |41|23|
             *   +--+--+
             */

            const string challengechars = "0123456789ABCDEF";
            const string calculatedchars = "⓪①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮";

            StringBuilder sb = new StringBuilder();
            sb.Append('+');
            for (int x = 0; x < BlockHeight; x++) sb.Append('-', BlockWidth).Append('+');
            sb.AppendLine();
            for (int br = 0; br < BlockWidth; br++)
            {
                for (int rr = 0; rr < BlockHeight; rr++)
                {
                    int row = br * BlockHeight + rr;
                    sb.Append('|');
                    for (int bc = 0; bc < BlockHeight; bc++)
                    {
                        for (int cc = 0; cc < BlockWidth; cc++)
                        {
                            int col = bc * BlockWidth + cc;
                            var cell = GetCellByRowColumn(row, col);
                            if (cell.GivenValue.HasValue)
                                sb.Append(challengechars[cell.GivenValue.Value]);
                            else if (cell.CalculatedValue.HasValue)
                                sb.Append(calculatedchars[cell.CalculatedValue.Value]);
                            else
                                sb.Append('.');
                        }
                        sb.Append('|');
                    }
                    sb.AppendLine();
                }
                sb.Append('+');
                for (int x = 0; x < BlockHeight; x++) sb.Append('-', BlockWidth).Append('+');
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the challenge of this instance (blanks for calculated values).
        /// </summary>
        /// <returns></returns>
        public string ToChallengeString()
        {
            /*
             *   +--+--+
             *   |12|34|
             *   |34|12|
             *   +--+--+
             *   |23|41|
             *   |41|23|
             *   +--+--+
             */
            const string chars = "0123456789ABCDEF";

            StringBuilder sb = new StringBuilder();
            sb.Append('+');
            for (int x = 0; x < BlockHeight; x++) sb.Append('-', BlockWidth).Append('+');
            sb.AppendLine();
            for (int br = 0; br < BlockWidth; br++)
            {
                for (int rr = 0; rr < BlockHeight; rr++)
                {
                    int row = br * BlockHeight + rr;
                    sb.Append('|');
                    for (int bc = 0; bc < BlockHeight; bc++)
                    {
                        for (int cc = 0; cc < BlockWidth; cc++)
                        {
                            int col = bc * BlockWidth + cc;
                            var cell = GetCellByRowColumn(row, col);
                            if (cell.GivenValue.HasValue)
                                sb.Append(chars[cell.GivenValue.Value]);
                            else
                                sb.Append('.');
                        }
                        sb.Append('|');
                    }
                    sb.AppendLine();
                }
                sb.Append('+');
                for (int x = 0; x < BlockHeight; x++) sb.Append('-', BlockWidth).Append('+');
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the current status of the grid. 
        /// For each position: preceded by '=': value given by challenge; preceded by '+': calculated value; else the available values. 
        /// </summary>
        /// <returns></returns>
        public string ToStatusString()
        {
            var maxlength = AllCells().Select(c => c.HasGivenOrCalculatedValue ? 0 : c.AvailableValues.Count).Max();

            const string chars = "0123456789ABCDEF";

            StringBuilder sb = new StringBuilder();
            sb.Append('+');
            for (int x = 0; x < BlockHeight; x++) sb.Append('-', BlockWidth * (maxlength + 1)).Append('+');
            sb.AppendLine();
            for (int br = 0; br < BlockWidth; br++)
            {
                for (int rr = 0; rr < BlockHeight; rr++)
                {
                    int row = br * BlockHeight + rr;
                    sb.Append('|');
                    for (int bc = 0; bc < BlockHeight; bc++)
                    {
                        for (int cc = 0; cc < BlockWidth; cc++)
                        {
                            int col = bc * BlockWidth + cc;
                            var cell = GetCellByRowColumn(row, col);
                            if (cell.GivenValue.HasValue)
                            {
                                sb.Append('=').Append(chars[cell.GivenValue.Value]).Append(' ', maxlength - 1);
                            }
                            else if (cell.CalculatedValue.HasValue)
                            {
                                sb.Append('+').Append(chars[cell.CalculatedValue.Value]).Append(' ', maxlength - 1);
                            }
                            else
                            {
                                //sb.Append('.');
                                foreach (var value in cell.AvailableValues)
                                {
                                    sb.Append(chars[value]);
                                }
                                sb.Append(' ', maxlength - cell.AvailableValues.Count + 1);
                            }
                        }
                        sb.Append('|');
                    }
                    sb.AppendLine();
                }
                sb.Append('+');
                for (int x = 0; x < BlockHeight; x++) sb.Append('-', BlockWidth * (maxlength + 1)).Append('+');
                sb.AppendLine();
            }

            return sb.ToString();

        }

    }
}
