using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SudokuX.UI.Common.Enums;

namespace SudokuX.UI.Controls
{
    public class BorderToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bt = (BorderType)value;
            switch (bt)
            {
                case BorderType.Block:
                    return Brushes.SlateGray;
                case BorderType.Special:
                    return Brushes.DarkSlateGray;
                case BorderType.Regular:
                    return Brushes.Transparent;
            }

            throw new InvalidOperationException("Unknown enum value");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
