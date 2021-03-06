﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SudokuX.Solver.Support.Enums;
using SudokuX.UI.Common;
using SudokuX.UI.Common.Enums;
using SudokuX.UI.Controls;
using System.Threading.Tasks;
using SudokuX.Solver;

namespace SudokuX.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private ValueSelectionMode _selectionMode;
        private SudokuBoard _board;
        private string _selectedButtonValue;
        private int _selectedCellRow, _selectedCellColumn;
        private bool _isPenSelected = true;
        private bool _isFinished;
        private ResourceDictionary _dict;

        public MainWindow()
        {
            InitializeComponent();
            NewGame(null, null);

            SetLanguageDictionary();
        }

        void QuitClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private BoardSize SelectedBoardSize
        {
            get
            {
                var tag = ((ComboBoxItem)BoardSize.SelectedItem).Tag.ToString();
                switch (tag)
                {
                    case "4x4":
                        return Solver.Support.Enums.BoardSize.Board4;
                    case "6x6":
                        return Solver.Support.Enums.BoardSize.Board6;
                    case "8x8R":
                        return Solver.Support.Enums.BoardSize.Board8Row;
                    case "8x8C":
                        return Solver.Support.Enums.BoardSize.Board8Column;
                    case "8x8M":
                        return Solver.Support.Enums.BoardSize.Board8Mix;
                    case "9x9":
                        return Solver.Support.Enums.BoardSize.Board9;
                    case "9Diag":
                        return Solver.Support.Enums.BoardSize.Board9X;
                    case "12x12":
                        return Solver.Support.Enums.BoardSize.Board12;
                    case "16x16":
                        return Solver.Support.Enums.BoardSize.Board16;
                    case "Irr9":
                        return Solver.Support.Enums.BoardSize.Irregular9;
                    case "Irr6":
                        return Solver.Support.Enums.BoardSize.Irregular6;
                    case "Hyp9":
                        return Solver.Support.Enums.BoardSize.Hyper9;
                }
                throw new InvalidOperationException("Unknown board size: " + tag);
            }
        }

        private Difficulty SelectedDifficulty
        {
            get
            {
                return DifficultyLevel.SelectedItem != null
                    ? ((ComboBoxItem)DifficultyLevel.SelectedItem).Tag as Difficulty? ?? Difficulty.Normal
                    : Difficulty.Normal;
            }
        }

        private async void NewGame(object sender, RoutedEventArgs e)
        {
            NewGameButton.IsEnabled = false;
            ShowPencilmarks.IsChecked = false;
            ShowPencilmarks.IsEnabled = true;
            GridScoreLabel.Text = "🔁";

            await Task.Delay(10);

            _board = new SudokuBoard(SelectedBoardSize, SelectedDifficulty);

            if (_board != null)
            {
                GridPlaceholder.Child = _board;

                _board.DoneCreating += board_DoneCreating;
                _board.CellClicked += board_CellClicked;
                _board.BoardIsFinished += board_BoardIsFinished;
                CreationProgress.Visibility = Visibility.Visible;
                CreationProgress.IsIndeterminate = true;
                _board.Create();
                _board.ValueCounts.Add(new ValueCount(" "));
                ButtonPanel.ItemsSource = SplitInRows(_board.ValueCounts, _board.BoardSize);

                const int buttonWidth = 65;
                switch(_board.BoardSize)
                {
                    case Solver.Support.Enums.BoardSize.Board4:
                        RightPanel.Width = buttonWidth * 3; // some extra space here
                        break;
                    case Solver.Support.Enums.BoardSize.Board6:
                    case Solver.Support.Enums.BoardSize.Irregular6:
                    case Solver.Support.Enums.BoardSize.Board9:
                    case Solver.Support.Enums.BoardSize.Board9X:
                    case Solver.Support.Enums.BoardSize.Hyper9:
                    case Solver.Support.Enums.BoardSize.Irregular9:
                        RightPanel.Width = buttonWidth * 3;
                        break;
                    default: // 12, 16
                        RightPanel.Width = buttonWidth * 4;
                        break;
                }

                _isFinished = false;
            }
        }

        void board_BoardIsFinished(object sender, EventArgs e)
        {
            _board.HighlightValue(null);
            ResetButtonsAndCellSelections();

            _isFinished = true;
        }

        private List<List<T>> SplitInRows<T>(IList<T> list, BoardSize boardSize)
        {
            var result = new List<List<T>>();

            var width = boardSize.BlockWidth();
            //var height = boardSize.BlockHeight();

            for (int r = 0; r < Math.Ceiling(list.Count / (double)width); r++)
            {
                var line = new List<T>();
                line.AddRange(list.Skip(r * width).Take(width));
                result.Add(line);
            }

            return result;
        }

        private string GetTranslation(string key)
        {
            return _dict[key].ToString();
        }

        void board_DoneCreating(object sender, EventArgs e)
        {
            CreationProgress.IsIndeterminate = true;
            CreationProgress.Visibility = Visibility.Hidden;
            NewGameButton.IsEnabled = true;
            GridScoreLabel.Text = String.Format(GetTranslation("GridScoreLabel"), _board.GridScore, _board.WeightedGridScore);
        }

        /// <summary>
        /// Is this boardsize too easy for pencilmarks?
        /// </summary>
        /// <param name="size">The boardsize.</param>
        /// <returns></returns>
        private bool TooEasy(BoardSize size)
        {
            return size == Solver.Support.Enums.BoardSize.Board4 ||
                   size == Solver.Support.Enums.BoardSize.Board6;
        }

        /// <summary>
        /// Handles the OnClick event of the ShowPencilmarks checkbox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void ShowPencilmarks_OnClick(object sender, RoutedEventArgs e)
        {
            var board = (SudokuBoard)GridPlaceholder.Child;

            if (TooEasy(board.BoardSize))
            {
                string msg = GetTranslation("ShowPencil-TooEasy");
                ShowPencilmarks.IsChecked = false;
                MessageBox.Show(msg);
                return;
            }

            var btn = (CheckBox)sender;
            board.ShowPencilMarks = btn.IsChecked.GetValueOrDefault();
            // show/hide pen/pencil selection box accordingly
            PenPencilSelection.Visibility = btn.IsChecked.GetValueOrDefault() ? Visibility.Visible : Visibility.Hidden;
            // reset to "pen" when UNchecked
            if (!btn.IsChecked.GetValueOrDefault()) _isPenSelected = true;
        }

        private void SetLanguageDictionary()
        {
            // http://www.codeproject.com/Articles/123460/Simplest-Way-to-Implement-Multilingual-WPF-Applica
            _dict = new ResourceDictionary();

            var cult = Thread.CurrentThread.CurrentUICulture.ToString();
            if (cult.StartsWith("nl"))
            {
                _dict.Source = new Uri(@"Resources/StringResources.nl.xaml", UriKind.Relative);
            }
            else
            {
                _dict.Source = new Uri(@"Resources/StringResources.xaml", UriKind.Relative);
            }

            Resources.MergedDictionaries.Add(_dict);
        }

        /// <summary>
        /// Handles the OnMouseDown event of the GameWindow control, outside of the game area or value buttons.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void GameWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            _selectionMode = ValueSelectionMode.None;

            ResetButtonsAndCellSelections();
        }

        /// <summary>
        /// Handles the OnClick event of the SelectButton control: select a "digit".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void SelectButton_OnClick(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag.ToString();
            e.Handled = true;

            switch (_selectionMode)
            {
                case ValueSelectionMode.None:
                    _selectionMode = ValueSelectionMode.ButtonFirst;
                    goto case ValueSelectionMode.ButtonFirst;

                case ValueSelectionMode.ButtonFirst:
                    HighlightButton(tag);
                    break;

                case ValueSelectionMode.CellFirst:
                    await SetCellToValue(_selectedCellRow, _selectedCellColumn, tag);
                    break;
            }
        }

        /// <summary>
        /// Handles the CellClicked event of the board control: select a cell in the grid.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellClickEventArgs"/> instance containing the event data.</param>
        private async void board_CellClicked(object sender, CellClickEventArgs e)
        {
            if (_isFinished)
                return;

            switch (_selectionMode)
            {
                case ValueSelectionMode.None:
                    _selectionMode = ValueSelectionMode.CellFirst;
                    goto case ValueSelectionMode.CellFirst;

                case ValueSelectionMode.ButtonFirst:
                    await SetCellToValue(e.Row, e.Column, _selectedButtonValue);
                    break;

                case ValueSelectionMode.CellFirst:
                    HighlightCell(e.Row, e.Column);
                    break;
            }
        }

        private void ResetButtonsAndCellSelections()
        {
            foreach (var vc in _board.ValueCounts)
            {
                vc.IsSelected = false;
            }

            _board.DeselectAllCells();
        }

        private bool HighlightButton(string value)
        {
            ResetButtonsAndCellSelections();
            var cnt = _board.ValueCounts.SingleOrDefault(vc => vc.Value == value);
            if (cnt != null)
            {
                _selectedButtonValue = value;
                cnt.IsSelected = true;
                _board.HighlightValue(value);
                return true;
            }

            return false;
        }

        private void HighlightCell(int row, int column)
        {
            _selectedCellRow = row;
            _selectedCellColumn = column;

            _board.SelectCell(row, column);
        }

        private async Task SetCellToValue(int row, int column, string value)
        {
            if (_isPenSelected)
            {
                await _board.SetCellToValue(row, column, value);
            }
            else
            {
                _board.ToggleAvailableValue(row, column, value);
            }

            if (_selectionMode == ValueSelectionMode.ButtonFirst && !_isFinished)
            {
                _board.HighlightValue(value);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_board != null)
                {
                    _board.Dispose();
                    _board = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void UndoButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_isFinished)
            {
                _board.Undo();
            }
        }

        private void PenPencil_OnClick(object sender, RoutedEventArgs e)
        {
            var btn = (RadioButton)sender;
            var tag = btn.Tag.ToString();

            if (!TooEasy(_board.BoardSize))
            {
                _isPenSelected = tag == "Pen";
            }
        }

        private void MainWindow_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                // tab = toggle pen/pencil (space is handled by control, return ??)
                // backspace = undo
                // F1 = help (webpage)
                // other: pass through to board
                if (e.Key == Key.Return || e.Key == Key.Tab)
                {
                    if (ShowPencilmarks.IsVisible && ShowPencilmarks.IsChecked == true)
                    {
                        if (PenButton.IsChecked.GetValueOrDefault())
                        {
                            PencilButton.IsChecked = true;
                            PenPencil_OnClick(PencilButton, null);
                        }
                        else
                        {
                            PenButton.IsChecked = true;
                            PenPencil_OnClick(PenButton, null);
                        }
                    }
                    e.Handled = true;
                }
                else if (e.Key == Key.Back)
                {
                    UndoButton_OnClick(null, null);
                    e.Handled = true;
                }
                else if (e.Key == Key.F1)
                {
                    ShowHelp();
                    e.Handled = true;
                }
                else
                {
                    var key = e.Key.ToString();
                    if (key != "D" && key.StartsWith("D"))
                    {
                        // regular digit (D0 .. D9)
                        key = key.Substring(1);
                    }
                    else if (key.StartsWith("NumPad"))
                    {
                        // numpad digit (NumPad0 .. NumPad9)
                        key = key.Substring("NumPad".Length);
                    }

                    if (HighlightButton(key))
                    {
                        _selectionMode = ValueSelectionMode.ButtonFirst;
                        e.Handled = true;
                    }
                }
            }
        }

        private void BoardSize_Selected(object sender, RoutedEventArgs e)
        {
            // new board size has been selected, adjust difficulty dropdown accordingly
            var rangeTuple = Solver.GridConfigurator.GetLevelRange(SelectedBoardSize);

            if (DifficultyLevel != null)
            {
                DifficultyLevel.Items.Clear();

                // easy?
                if (rangeTuple.Item1 <= Difficulty.Easy)
                {
                    DifficultyLevel.Items.Add(new ComboBoxItem { Tag = Difficulty.Easy, Content = GetTranslation("Difficulty-Easy") });
                }

                // normal?
                if (rangeTuple.Item1 <= Difficulty.Normal && rangeTuple.Item2 >= Difficulty.Normal)
                {
                    DifficultyLevel.Items.Add(new ComboBoxItem { Tag = Difficulty.Normal, Content = GetTranslation("Difficulty-Standard"), IsSelected = true });
                }

                // harder?
                if (rangeTuple.Item1 <= Difficulty.Harder && rangeTuple.Item2 >= Difficulty.Harder)
                {
                    DifficultyLevel.Items.Add(new ComboBoxItem { Tag = Difficulty.Harder, Content = GetTranslation("Difficulty-Harder") });
                }
            }
        }

        private async void HintButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isFinished)
            {
                if (FindAndShowErrors())
                {
                    string msg = GetTranslation("Hint-Error");
                    MessageBox.Show(msg);
                    _selectionMode = ValueSelectionMode.ButtonFirst;
                    HighlightButton(" "); // erase button
                }
                else
                { 
                    // 1) prepare solver
                    // 2) execute solver, get first positive result
                    // 3) place result & highlight cell
                    var gridcopy = _board.CloneGridForSolver();
                    var solver = new GridSolver(GridConfigurator.GetGridSolvers(SelectedBoardSize, SelectedDifficulty));
                    var result = solver.SolveUntilFirstValue(gridcopy);
                    if (result != null)
                    {
                        // do it
                        if (!_isPenSelected)
                        {
                            PenButton.IsChecked = true;
                            PenPencil_OnClick(PenButton, null);
                        }
                        var trans = new ValueTranslator(SelectedBoardSize);
                        HighlightCell(result.TargetCell.Row, result.TargetCell.Column);
                        await Task.Delay(500);
                        await SetCellToValue(result.TargetCell.Row, result.TargetCell.Column, trans.ToChar(result.ExactValue.Value - gridcopy.MinValue));
                    }
                    // I don't expect errors, as the grid has been checked
                }
            }
        }

        /// <summary>
        /// Finds the errors and highlights them. Returns <c>true</c> when errors are found.
        /// </summary>
        /// <returns></returns>
        private bool FindAndShowErrors()
        {
            var invalid = false;
            // 1) get completed board that was used as challenge
            // 2) compare placed values with solution, mark differences
            var solution = _board.GetSolution();

            for(int r=0; r<solution.GridSize; r++)
            {
                for(int c=0; c<solution.GridSize; c++)
                {
                    var cell = _board.GetCell(r, c);

                    if (cell.HasValue)
                    {
                        var solcell = solution.GetCellByRowColumn(r, c);
                        if (cell.IntValue != solcell.GivenOrCalculatedValue - solution.MinValue)
                        {
                            cell.IsMarkedAsInvalid = true;
                            invalid = true;
                        }
                    }
                }
            }

            return invalid;
        }

        private void ShowHelp()
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("https://hdkesting.blob.core.windows.net/sudoku/index.html");
            System.Diagnostics.Process.Start(psi);
        }
    }
}
