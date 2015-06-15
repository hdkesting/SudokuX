using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SudokuX.UI.Controls
{
    /// <summary>
    /// Converts a <c>bool</c> to <c>Visiblity</c>
    /// </summary>
    /// <remarks>
    /// The built-in BoolToVisibilityConverter converts false to Collapsed - I want Hidden</remarks>
    public class BoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var b = (bool)value;
            return b ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (Visibility)value;

            return v == Visibility.Visible;
        }
    }
}
