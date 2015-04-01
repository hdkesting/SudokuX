using System.Windows;

namespace SudokuX.UI.Common
{
    public class PencilValue
    {
        public string Value { get; set; }

        public Visibility Visibility { get; set; }

        public PencilValue(string value)
        {
            Value = value;
            Visibility = Visibility.Visible;
        }
    }
}
