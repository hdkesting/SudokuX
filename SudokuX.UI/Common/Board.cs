using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using SudokuX.Solver.Support.Enums;
using SudokuX.UI.Annotations;
using SudokuX.UI.Common.Enums;

namespace SudokuX.UI.Common
{
    public class Board : INotifyPropertyChanged
    {
        private readonly List<List<Cell>> _rows;
        private readonly ObservableCollection<ValueCount> _valueCounts;
        private readonly GroupCollection _groups = new GroupCollection();

        private readonly ActionStack _actionStack = new ActionStack();

        private bool _isValidValue = true;
        private bool _isFinished;
        private bool _filling;
        private bool _showPencilMarks;
        private readonly ValueTranslator _translator;
        private string _activeButtonValue;

        //public event EventHandler<EventArgs> BoardIsFinished;

        public List<List<Cell>> GridRows { get { return _rows; } }

        public int GridSize { get; private set; }

        public bool HasDiagonals { get; private set; }

        public GroupCollection Groups { get { return _groups; } }

        public ObservableCollection<ValueCount> ValueCounts
        {
            get { return _valueCounts; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show pencil marks.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show pencil marks]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowPencilMarks
        {
            get { return _showPencilMarks; }
            set
            {
                if (_showPencilMarks != value)
                {
                    _showPencilMarks = value;
                    OnPropertyChanged();
                    foreach (var cell in EnumerateAllCells())
                    {
                        cell.ShouldShowPencilMarks = value;
                    }
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValidValue;
            }

            private set
            {
                if (_isValidValue != value)
                {
                    _isValidValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsFinished
        {
            get { return _isFinished; }
            set
            {
                if (_isFinished != value)
                {
                    _isFinished = value;
                    OnPropertyChanged();
                }
            }
        }

        public Cell this[int row, int col]
        {
            get
            {
                if (row < 0 || row >= GridSize) throw new ArgumentOutOfRangeException("row", row, "Invalid Row Index");
                if (col < 0 || col >= GridSize) throw new ArgumentOutOfRangeException("col", col, "Invalid Column Index");
                return _rows[row][col];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Board" /> class.
        /// </summary>
        /// <param name="boardSize">Size of the board.</param>
        public Board(BoardSize boardSize)
        {
            HasDiagonals = boardSize.HasDiagonals();

            _translator = new ValueTranslator(boardSize);
            GridSize = _translator.MaxValue + 1;


            _rows = new List<List<Cell>>();
            AddCells(_translator);

            _valueCounts = new ObservableCollection<ValueCount>();
            for (int v = 0; v < GridSize; v++)
            {
                var vc = new ValueCount(_translator.ToChar(v));
                _valueCounts.Add(vc);
            }
        }

        private void AddCells(ValueTranslator translator)
        {
            // add cells to board
            for (int row = 0; row < GridSize; row++)
            {
                List<Cell> cellRow = new List<Cell>();
                for (int col = 0; col < GridSize; col++)
                {
                    Cell c = new Cell(translator, String.Format("{0}|{1}", row, col));
                    c.PropertyChanged += CellPropertyChanged;
                    c.Board = this;
                    cellRow.Add(c);
                }
                _rows.Add(cellRow);
            }
        }

        public void SetBorders()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Cell c = _rows[row][col];
                    // North is row with lower ordinal, south higher
                    // West is col with lower ordinal, east higher
                    c.BorderNorth = row == 0 ? BorderType.Block : BorderBetween(c, _rows[row - 1][col]);
                    c.BorderSouth = row == GridSize - 1 ? BorderType.Block : BorderBetween(c, _rows[row + 1][col]);
                    c.BorderWest = col == 0 ? BorderType.Block : BorderBetween(c, _rows[row][col - 1]);
                    c.BorderEast = col == GridSize - 1 ? BorderType.Block : BorderBetween(c, _rows[row][col + 1]);
                }
            }
        }

        private static BorderType BorderBetween(Cell target, Cell neighbour)
        {
            if (target.BlockOrdinal != neighbour.BlockOrdinal)
                return BorderType.Block;

            if (target.BelongsToSpecialGroup && !neighbour.BelongsToSpecialGroup)
                return BorderType.Special; // I don't care if they are *different* special groups

            return BorderType.Regular;
        }

        async void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // check op "Value" changed
            // reset invalid vlag op gehele grid
            // check dan pas rijen en kolommen - markeer hele rij/kolom invalid bij fout
            if (e.PropertyName == "StringValue" && !_filling)
            {
                await Task.Yield();
                Recalculate();
            }
        }

        public void SetActiveButtonValue(string activeValue)
        {
            _activeButtonValue = activeValue;
        }

        private void Recalculate()
        {
            ResetInvalid();
            bool valid = true;

            RecalculateCounts();

            // redo all "PossibleValues" for entire grid (maybe the value was reset, maybe it was changed - in both cases you need to get rid of the old values)
            RedoPossibleValues();

            DeselectAllCells();
            if (!String.IsNullOrEmpty(_activeButtonValue))
            {
                HighlightValue(_activeButtonValue);
            }

            foreach (var cell in EnumerateAllCells())
            {
                cell.IsValid = cell.HasValue || cell.PossibleValues.Any();
            }

            foreach (var grp in Groups.AllGroups)
            {
                if (!GroupIsValid(grp))
                {
                    valid = false;
                    foreach (var cell in grp.ContainedCells)
                    {
                        cell.IsValid = false;
                    }
                }
            }

            IsValid = valid;

            IsFinished = IsValid && IsBoardFinished();
        }

        /// <summary>
        /// Unhighlights all cells.
        /// </summary>
        public void DeselectAllCells()
        {
            foreach (var cell in EnumerateAllCells())
            {
                cell.Highlighted = Highlight.None;
                cell.IsSelected = false;
            }
        }

        /// <summary>
        /// Highlights the given cell (plus siblings).
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        public void SelectCell(int row, int column)
        {
            // unselect previous cells
            DeselectAllCells();

            // select requested cell
            var cell = this[row, column];
            cell.IsSelected = true;

            // highlight all cells that share a group with this one
            foreach (var grp in _groups.Where(g => g.ContainedCells.Contains(cell)))
            {
                foreach (var sibling in grp.ContainedCells.Where(c => c != cell))
                {
                    sibling.Highlighted &= Highlight.Easy;
                }
            }
        }

        private bool IsBoardFinished()
        {
            return EnumerateAllCells().All(c => c.IntValue.HasValue);
        }

        private IEnumerable<Cell> EnumerateAllCells()
        {
            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    yield return _rows[x][y];
                }
            }
        }

        private void RedoPossibleValues()
        {
            // reset empty cells to full list of possibles
            foreach (var cell in EnumerateAllCells().Where(c => !c.IntValue.HasValue))
            {
                var localcell = cell;
                localcell.ResetPossibleValues();

                foreach (var sibling in _groups.GetGroupsByCell(cell)
                                            .SelectMany(g => g.ContainedCells)
                                            .Where(c => c != localcell && c.IntValue.HasValue))
                {
                    localcell.PossibleValues.Remove(sibling.StringValue);
                }
                localcell.UpdatePencilmarkStatus();
            }
        }

        private void RecalculateCounts()
        {
            foreach (var valueCount in _valueCounts)
            {
                valueCount.Count = GridSize;
            }

            foreach (var cell in EnumerateAllCells())
            {
                if (cell.IntValue.HasValue)
                {
                    var vc = _valueCounts.Single(x => x.Value == cell.StringValue);
                    vc.Count--;
                }

            }
        }

        private void ResetInvalid()
        {
            foreach (var cell in EnumerateAllCells())
            {
                cell.IsValid = true;
            }

            _isValidValue = true;
        }

        private bool GroupIsValid(Group grp)
        {
            bool[] used = new bool[GridSize];
            foreach (var cell in grp.ContainedCells)
            {
                if (cell.IntValue.HasValue)
                {
                    if (used[cell.IntValue.Value])
                    {
                        _isValidValue = false;
                        return false;
                    }

                    used[cell.IntValue.Value] = true;
                }
            }

            return true;

        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void StartFilling()
        {
            _filling = true;
        }

        public void DoneFilling()
        {
            _filling = false;
            Recalculate();
        }

        /// <summary>
        /// Highlights the cells that contain the supplied value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void HighlightValue(string value)
        {
            foreach (var cell in EnumerateAllCells().Where(c => c.HasValue && c.StringValue == value))
            {
                cell.Highlighted |= Highlight.Pen;
            }

            if (ShowPencilMarks)
            {
                foreach (var cell in EnumerateAllCells().Where(c => !c.HasValue && c.HasPencilMark(value)))
                {
                    cell.Highlighted |= Highlight.Pencil;
                }
            }
        }

        public async Task SetCellToValue(int row, int column, string value)
        {
            var cell = this[row, column];
            if (!cell.IsReadOnly)
            {
                if (cell.StringValue == value)
                {
                    var oldval = cell.IntValue.GetValueOrDefault();
                    cell.StringValue = "";
                    // add removal to stack, unless the previous item was the addition
                    _actionStack.PushAction(new PerformedAction(cell) { IsValueSet = false, IsRealValue = true, IntValue = oldval });
                }
                else
                {
                    cell.StringValue = value;
                    // add the addition to the stack
                    _actionStack.PushAction(new PerformedAction(cell) { IsValueSet = true, IsRealValue = true, IntValue = cell.IntValue.GetValueOrDefault() });

                    var fullgroups = _groups
                                        .Where(g => g.ContainedCells.Contains(cell))
                                        .Where(g => g.ContainedCells.All(c => c.HasValue))
                                        .ToList();
                    if (fullgroups.Any())
                    {
                        // flash all finished groups
                        var tasks = fullgroups.Select(g => FlashGroup(g)).ToArray();
                        await Task.WhenAll(tasks);
                    }
                }
            }
        }

        private async Task FlashGroup(Group group)
        {
            const int delay = 50;

            // set group highlight
            foreach(var cell in group.ContainedCells)
            {
                await Task.Delay(delay);
                cell.Highlighted |= Highlight.Group;
            }

            await Task.Delay(delay);

            // remove group highlight
            foreach (var cell in group.ContainedCells)
            {
                await Task.Delay(delay);
                cell.Highlighted = cell.Highlighted & ~Highlight.Group;
            }
        }

        public async Task FlashAllGroups()
        {
            List<Task> tasks = new List<Task>();
            int i = 0;
            foreach(var grp in Groups)
            {
                i += 1;
                tasks.Add(Task.Delay(i*150).ContinueWith(_ => FlashGroup(grp)));
            }

            await Task.WhenAll(tasks);
        }

        public void Undo()
        {
            if (_actionStack.HasItems)
            {
                var undo = _actionStack.PopAction();
                if (undo.IsRealValue)
                {
                    // not a pencil value
                    if (undo.IsValueSet)
                    {
                        // remove this value
                        undo.Cell.StringValue = "";
                    }
                    else
                    {
                        // re-set this value
                        undo.Cell.IntValue = undo.IntValue;
                    }
                }
                else
                {
                    // pencil-change 
                    var val = _translator.ToChar(undo.IntValue);
                    undo.Cell.SetPencilMark(val, !undo.IsValueSet, true);
                }
            }
        }

        public void ToggleAvailableValue(int row, int column, string value)
        {
            var cell = this[row, column];
            int intval = _translator.ToInt(value);

            if (!cell.IsReadOnly && !cell.HasValue)
            {
                if (cell.HasPencilMark(value))
                {
                    cell.SetPencilMark(value, false, true);
                    cell.Highlighted = cell.Highlighted & ~Highlight.Pencil; // remove any pencil highlight
                    _actionStack.PushAction(new PerformedAction(cell) { IsValueSet = false, IsRealValue = false, IntValue = intval });
                }
                else
                {
                    cell.SetPencilMark(value, true, true);
                    _actionStack.PushAction(new PerformedAction(cell) { IsValueSet = true, IsRealValue = false, IntValue = intval });
                }
            }
        }
    }
}
