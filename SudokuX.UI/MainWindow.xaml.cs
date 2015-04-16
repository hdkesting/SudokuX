using System;
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

        private void NewGame(object sender, RoutedEventArgs e)
        {
            NewGameButton.IsEnabled = false;
            ShowPencilmarks.IsChecked = false;
            ShowPencilmarks.IsEnabled = true;

            _board = null;
            switch (((ComboBoxItem)BoardSize.SelectedItem).Tag.ToString())
            {
                case "4x4":
                    _board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board4);
                    break;
                case "6x6":
                    _board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board6);
                    break;
                case "9x9":
                    _board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board9);
                    break;
                case "9Diag":
                    _board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board9X);
                    break;
                case "12x12":
                    _board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board12);
                    break;
                case "16x16":
                    _board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board16);
                    break;
                case "Irr9":
                    _board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board9Irregular);
                    break;
                case "Irr6":
                    _board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board6Irregular);
                    break;
            }

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
            }
        }

        void board_BoardIsFinished(object sender, EventArgs e)
        {
            ResetButtonsAndCells();
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

        void board_DoneCreating(object sender, EventArgs e)
        {
            CreationProgress.IsIndeterminate = true;
            CreationProgress.Visibility = Visibility.Hidden;
            NewGameButton.IsEnabled = true;
            GridScoreLabel.Text = String.Format(_dict["GridScoreLabel"].ToString(), _board.GridScore, _board.WeightedGridScore);
        }

        private void ShowPencilmarks_OnClick(object sender, RoutedEventArgs e)
        {
            var board = (SudokuBoard)GridPlaceholder.Child;

            if (board.BoardSize == Solver.Support.Enums.BoardSize.Board4 ||
                board.BoardSize == Solver.Support.Enums.BoardSize.Board6)
            {
                string msg = _dict["ShowPencil-TooEasy"].ToString();
                ShowPencilmarks.IsChecked = null;
                MessageBox.Show(msg);
                return;
            }

            var btn = (CheckBox)sender;
            board.ShowPencilMarks = btn.IsChecked.GetValueOrDefault();
        }

        private ResourceDictionary _dict;
        private void SetLanguageDictionary()
        {
            // http://www.codeproject.com/Articles/123460/Simplest-Way-to-Implement-Multilingual-WPF-Applica
            _dict = new ResourceDictionary();

            var cult = Thread.CurrentThread.CurrentCulture.ToString();
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

        private void GameWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            _selectionMode = ValueSelectionMode.None;

            ResetButtonsAndCells();
        }

        private void SelectButton_OnClick(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag.ToString();
            e.Handled = true;

            switch (_selectionMode)
            {
                case ValueSelectionMode.None:
                    _selectionMode = ValueSelectionMode.ButtonFirst;
                    goto case ValueSelectionMode.ButtonFirst;

                case ValueSelectionMode.ButtonFirst:
                    ResetButtonsAndCells();
                    HighlightButton(tag);
                    break;

                case ValueSelectionMode.CellFirst:
                    SetCellToValue(_selectedCellRow, _selectedCellColumn, tag);
                    break;
            }
        }

        void board_CellClicked(object sender, Common.CellClickEventArgs e)
        {
            switch (_selectionMode)
            {
                case ValueSelectionMode.None:
                    _selectionMode = ValueSelectionMode.CellFirst;
                    goto case ValueSelectionMode.CellFirst;

                case ValueSelectionMode.ButtonFirst:
                    SetCellToValue(e.Row, e.Column, _selectedButtonValue);
                    break;

                case ValueSelectionMode.CellFirst:
                    ResetButtonsAndCells();
                    HighlightCell(e.Row, e.Column);
                    break;
            }
        }

        private void ResetButtonsAndCells()
        {
            foreach (var vc in _board.ValueCounts)
            {
                vc.IsSelected = false;
            }

            _board.DeselectAllCells();
        }

        private void HighlightButton(string value)
        {
            _selectedButtonValue = value;
            var cnt = _board.ValueCounts.SingleOrDefault(vc => vc.Value == value);
            if (cnt != null)
            {
                cnt.IsSelected = true;
            }

            _board.HighlightValue(value);
        }

        private void HighlightCell(int row, int column)
        {
            _selectedCellRow = row;
            _selectedCellColumn = column;

            _board.SelectCell(row, column);
        }

        private void SetCellToValue(int row, int column, string value)
        {
            _board.SetCellToValue(row, column, value);

            if (_selectionMode == ValueSelectionMode.ButtonFirst)
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
    }
}
