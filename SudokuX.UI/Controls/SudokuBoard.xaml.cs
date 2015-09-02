using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using SudokuX.Solver;
using SudokuX.Solver.Core;
using SudokuX.Solver.Support;
using SudokuX.Solver.Support.Enums;
using SudokuX.UI.Common;

namespace SudokuX.UI.Controls
{
    /// <summary>
    /// Interaction logic for SudokuBoard.xaml
    /// </summary>
    public partial class SudokuBoard : UserControl, IDisposable
    {
        private readonly BoardSize _boardSize;
        private readonly Board _gameBoard;

        private readonly BackgroundWorker _boardCreator = new BackgroundWorker();
        private ISudokuGrid _creatorGrid;
        private int _counter = 50;

        public SudokuBoard(BoardSize boardSize)
        {
            _boardSize = boardSize;
            InitializeComponent();
            _gameBoard = new Board(boardSize);
        }

        public event EventHandler<EventArgs> BoardIsFinished;

        public ObservableCollection<ValueCount> ValueCounts { get; private set; }

        public bool ShowPencilMarks
        {
            get { return _gameBoard.ShowPencilMarks; }
            set { _gameBoard.ShowPencilMarks = value; }
        }

        public BoardSize BoardSize { get { return _boardSize; } }

        public int GridScore { get; set; }

        public double WeightedGridScore { get; set; }

        public void Create()
        {
            _boardCreator.DoWork += (sender, args) => CreateChallenge(_boardSize, sender, args);
            _boardCreator.RunWorkerCompleted += _boardCreator_RunWorkerCompleted;
            _boardCreator.WorkerReportsProgress = true;
            _boardCreator.ProgressChanged += _boardCreator_ProgressChanged;


            Cursor = Cursors.AppStarting;
            _boardCreator.RunWorkerAsync();
            MainList.DataContext = _gameBoard;
            ValueCounts = _gameBoard.ValueCounts;

            _gameBoard.PropertyChanged += _gameBoard_PropertyChanged;
        }

        async void _gameBoard_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsFinished" && _gameBoard.IsFinished)
            {
                // All done!
                await Task.Delay(100);
                var fin = BoardIsFinished;
                if (fin != null)
                {
                    fin(this, EventArgs.Empty);
                }

                var sb = (Storyboard)this.FindResource("FinishAnimation");
                sb.Begin();
            }
        }

        public event EventHandler<ProgressChangedEventArgs> Progress;

        public event EventHandler<EventArgs> DoneCreating;

        private void _boardCreator_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Debug.WriteLine("=========================== {0}% ===========================", e.ProgressPercentage);
            var progress = Progress;
            if (progress != null)
            {
                progress(sender, e);
            }

            _counter++;
            if (_counter % 70 == 0)
            {
                FillBoard();
            }
        }

        private void _boardCreator_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FillBoard();

            Cursor = Cursors.Arrow;

            var done = DoneCreating;
            if (done != null)
            {
                done(sender, new EventArgs());
            }
        }

        private void CreateChallenge(BoardSize boardSize, object sender, DoWorkEventArgs args)
        {
            EventHandler<ProgressEventArgs> progress =
                (snd, progressArgs) => ((BackgroundWorker)sender).ReportProgress(progressArgs.PercentageDone);

            ChallengeCreator creator;
            bool success;
            do
            {
                creator = new ChallengeCreator(boardSize);
                _creatorGrid = creator.Grid; // block structure (possibly irregular) has been created by now

                AttachBlocks();

                creator.Progress += progress;

                success = creator.CreateChallenge();
            } while (!success);

            // solve the newly created board, to get it's score
            var testgrid = _creatorGrid.CloneBoardAsChallenge();
            var solver = new GridSolver(creator.Solvers);
            solver.Solve(testgrid);
            GridScore = solver.GridScore;
            WeightedGridScore = solver.WeightedGridScore;

            creator.Progress -= progress;
        }

        private void FillBoard()
        {
            _gameBoard.StartFilling();
            for (int row = 0; row < _creatorGrid.GridSize; row++)
            {
                for (int col = 0; col < _creatorGrid.GridSize; col++)
                {
                    var challcell = _creatorGrid.GetCellByRowColumn(row, col);
                    var boardcell = _gameBoard[row, col];
                    if (challcell.GivenValue.HasValue)
                    {
                        boardcell.IntValue = challcell.GivenValue.Value - _creatorGrid.MinValue; // make 0-based
                        boardcell.IsReadOnly = true;
                    }
                    else
                    {
                        boardcell.IntValue = null;
                        boardcell.IsReadOnly = false;
                    }
                }
            }
            _gameBoard.DoneFilling();
        }

        private void AttachBlocks()
        {
            ClearAllGroups();
            for (int row = 0; row < _creatorGrid.GridSize; row++)
            {
                for (int col = 0; col < _creatorGrid.GridSize; col++)
                {
                    var challcell = _creatorGrid.GetCellByRowColumn(row, col);
                    var boardcell = _gameBoard[row, col];

                    foreach (var containingGroup in challcell.ContainingGroups)
                    {
                        var grp = _gameBoard.Groups.GetGroup(containingGroup.GroupType, containingGroup.Ordinal);
                        grp.ContainedCells.Add(boardcell);
                    }

                    var block = challcell.ContainingGroups.First(g => g.GroupType == GroupType.Block);
                    boardcell.BlockOrdinal = block.Ordinal;
                    boardcell.BelongsToSpecialGroup = challcell.ContainingGroups.Any(g => g.GroupType == GroupType.Special);

                    double hue = 1.7 / _creatorGrid.GridSize * block.Ordinal;
                    Color color = Utils.FromHsla(hue, 0.7, 0.7);
                    if (_boardSize.IsIrregular())
                    {
                        color = Utils.FromHsla(hue, 0.8, 0.7);
                    }
                    else if (boardcell.BelongsToSpecialGroup)
                    {
                        color = Utils.FromHsla(hue, 0.7, 0.3);
                    }

                    boardcell.BlockColor = color;
                }
            }

            _gameBoard.SetBorders();
        }

        private void ClearAllGroups()
        {
            _gameBoard.Groups.Clear();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_boardCreator != null)
                {
                    _boardCreator.CancelAsync(); // moet ik niet eerst checken of dit nog bezig is?
                    _boardCreator.Dispose(); // moet ik niet wachten tot de berekening klaar/gestopt is?           
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public event EventHandler<CellClickEventArgs> CellClicked;

        /// <summary>
        /// Handles the OnClick event of the CellButton control: the cell was clicked to select it or fill it in
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void CellButton_OnClick(object sender, RoutedEventArgs e)
        {
            await Task.Yield();

            var tag = ((Button)sender).Tag.ToString().Split('|');
            int row = Convert.ToInt32(tag[0]);
            int col = Convert.ToInt32(tag[1]);

            var cell = _gameBoard[row, col];
            if (!cell.IsReadOnly) // ignore for given cells
            {
                var evt = CellClicked;
                if (evt != null)
                {
                    evt(this, new CellClickEventArgs(row, col));
                }
            }

            e.Handled = true;
        }

        public void DeselectAllCells()
        {
            _gameBoard.DeselectAllCells();
        }

        public void SelectCell(int row, int column)
        {
            _gameBoard.SelectCell(row, column);
        }

        public void HighlightValue(string value)
        {
            _gameBoard.SetActiveButtonValue(value);
            _gameBoard.HighlightValue(value);
        }

        public async Task SetCellToValue(int row, int column, string value)
        {
            await _gameBoard.SetCellToValue(row, column, value);
        }

        public void Undo()
        {
            _gameBoard.Undo();
        }

        internal void ToggleAvailableValue(int row, int column, string value)
        {
            _gameBoard.ToggleAvailableValue(row, column, value);
        }
    }
}
