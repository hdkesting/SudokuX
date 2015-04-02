using System;
using System.Windows;
using System.Windows.Controls;
using SudokuX.UI.Common;
using SudokuX.UI.Controls;

namespace SudokuX.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NewGame(null, null);
        }

        void QuitClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            NewGameButton.IsEnabled = false;
            ShowPencilmarks.IsChecked = false;

            SudokuBoard board = null;
            switch (((ComboBoxItem)BoardSize.SelectedItem).Tag.ToString())
            {
                case "4x4":
                    board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board4);
                    break;
                case "6x6":
                    board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board6);
                    break;
                case "9x9":
                    board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board9);
                    break;
                case "9X":
                    board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board9X);
                    break;
                case "12x12":
                    board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board12);
                    break;
                case "16x16":
                    board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board16);
                    break;
                case "Irr9":
                    board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board9Irregular);
                    break;
                case "Irr6":
                    board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board6Irregular);
                    break;
            }

            if (board != null)
            {
                GridPlaceholder.Child = board;
                //board.Progress += board_Progress;
                board.Done += board_Done;
                CreationProgress.Visibility = Visibility.Visible;
                CreationProgress.IsIndeterminate = true;
                board.Create();
                SummaryPanel.ItemsSource = board.ValueCounts;
            }
        }

        void board_Done(object sender, System.EventArgs e)
        {
            CreationProgress.IsIndeterminate = true;
            CreationProgress.Visibility = Visibility.Hidden;
            NewGameButton.IsEnabled = true;
        }

        //void board_Progress(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        //{
        //    if (CreationProgress.Value > e.ProgressPercentage)
        //    {
        //        CreationProgress.IsIndeterminate = true;
        //        CreationProgress.Value -= 0.1;
        //    }
        //    else
        //    {
        //        CreationProgress.IsIndeterminate = false;
        //        CreationProgress.Value = e.ProgressPercentage;
        //    }
        //}

        private void ToggleHighlight(object sender, RoutedEventArgs e)
        {
            var btn = (CheckBox)sender;

            var val = btn.Tag.ToString();

            var board = (SudokuBoard)GridPlaceholder.Child;

            board.ToggleHighlight(val, btn.IsChecked.GetValueOrDefault());
        }

        private void ShowPencilmarks_OnClick(object sender, RoutedEventArgs e)
        {
            var board = (SudokuBoard)GridPlaceholder.Child;
            var btn = (CheckBox)sender;
            board.ShowPencilMarks = btn.IsChecked.GetValueOrDefault();
        }
    }
}
