using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SudokuX.Solver.Support.Enums;
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

            SudokuBoard board = null;
            switch (((ComboBoxItem)BoardSize.SelectedItem).Tag.ToString())
            {
                case "4x4":
                    board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board4);
                    //ShowPencilmarks.IsEnabled = false;
                    break;
                case "6x6":
                    board = new SudokuBoard(Solver.Support.Enums.BoardSize.Board6);
                    //ShowPencilmarks.IsEnabled = false;
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

                board.DoneCreating += board_DoneCreating;
                CreationProgress.Visibility = Visibility.Visible;
                CreationProgress.IsIndeterminate = true;
                board.Create();
                //SummaryPanel.ItemsSource = board.ValueCounts;
                ButtonPanel.ItemsSource = SplitInRows(board.ValueCounts, board.BoardSize);
            }
        }

        private List<List<T>> SplitInRows<T>(IList<T> list, BoardSize boardSize)
        {
            var result = new List<List<T>>();

            var width = boardSize.BlockWidth();
            var height = boardSize.BlockHeight();

            for (int r = 0; r < height; r++)
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
        }

        //private void ToggleHighlight(object sender, RoutedEventArgs e)
        //{
        //    var btn = (CheckBox)sender;

        //    var val = btn.Tag.ToString();

        //    var board = (SudokuBoard)GridPlaceholder.Child;

        //    board.ToggleHighlight(val, btn.IsChecked.GetValueOrDefault());
        //}

        private void ShowPencilmarks_OnClick(object sender, RoutedEventArgs e)
        {
            var board = (SudokuBoard)GridPlaceholder.Child;

            if (board.BoardSize == Solver.Support.Enums.BoardSize.Board4 ||
                board.BoardSize == Solver.Support.Enums.BoardSize.Board6)
            {
                string msg = dict["ShowPencil-TooEasy"].ToString();
                MessageBox.Show(msg);
                return;
            }

            var btn = (CheckBox)sender;
            board.ShowPencilMarks = btn.IsChecked.GetValueOrDefault();
        }

        private ResourceDictionary dict;
        private void SetLanguageDictionary()
        {
            // http://www.codeproject.com/Articles/123460/Simplest-Way-to-Implement-Multilingual-WPF-Applica
            dict = new ResourceDictionary();

            var cult = Thread.CurrentThread.CurrentCulture.ToString();
            if (cult.StartsWith("nl"))
            {
                dict.Source = new Uri(@"Resources/StringResources.nl.xaml", UriKind.Relative);
            }
            else
            {
                dict.Source = new Uri(@"Resources/StringResources.xaml", UriKind.Relative);
            }

            Resources.MergedDictionaries.Add(dict);
        }

        private void GameWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("GameWindow_OnMouseDown");
            e.Handled = true;
        }

        private void SelectButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("SelectButton_OnClick");
            e.Handled = true;
        }
    }
}
