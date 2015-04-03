using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using SudokuX.Solver;
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
        private int _counter = 500;

        public SudokuBoard(BoardSize boardSize)
        {
            _boardSize = boardSize;
            InitializeComponent();
            _gameBoard = new Board(boardSize);
        }

        public ObservableCollection<ValueCount> ValueCounts { get; private set; }
        public bool ShowPencilMarks
        {
            get { return _gameBoard.ShowPencilMarks; }
            set { _gameBoard.ShowPencilMarks = value; }
        }


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

            _gameBoard.BoardIsFinished += _gameBoard_BoardIsFinished;
        }

        void _gameBoard_BoardIsFinished(object sender, EventArgs e)
        {
            var sb = (Storyboard)this.FindResource("FinishAnimation");
            sb.Begin();
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
            if (_counter % 1000 == 0)
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

            var creator = new ChallengeCreator(boardSize);
            _creatorGrid = creator.Grid;

            AttachBlocks();

            creator.Progress += progress;

            creator.CreateChallenge();

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
                        boardcell.ReadOnly = true;
                    }
                    else
                    {
                        boardcell.IntValue = null;
                        boardcell.ReadOnly = false;
                    }
                }
            }
            _gameBoard.DoneFilling();
        }

        private void AttachBlocks()
        {
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
                    Color color = Utils.FromHsla(block.Ordinal * 0.15, 0.7, 0.7, 1.0);
                    if (_boardSize == BoardSize.Board9Irregular || _boardSize == BoardSize.Board6Irregular)
                    {
                        color = Utils.FromHsla(block.Ordinal * 0.22, 0.9, 0.5, 1.0);
                    }
                    else if (_gameBoard.HasDiagonals && (row == col || _creatorGrid.GridSize - 1 - row == col))
                    {
                        color = Utils.FromHsla(block.Ordinal * 0.17, 0.7, 0.3, 1.0);
                    }
                    boardcell.BlockColor = color;
                }
            }

        }

        private void Dispose(bool disposing)
        {
            _boardCreator.CancelAsync(); // moet ik niet eerst checken of dit nog bezig is?
            _boardCreator.Dispose(); // moet ik niet wachten tot de berekening klaar/gestopt is?           
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void ToggleHighlight(string val, bool highlight)
        {
            for (int row = 0; row < _creatorGrid.GridSize; row++)
            {
                for (int col = 0; col < _creatorGrid.GridSize; col++)
                {
                    var cell = _gameBoard[row, col];
                    if (cell.StringValue == val)
                    {
                        cell.IsHighlighted = highlight;
                    }
                    else if (!cell.HasValue)
                    {
                        cell.IsHighlighted = false;
                    }
                }
            }
        }
    }
}
