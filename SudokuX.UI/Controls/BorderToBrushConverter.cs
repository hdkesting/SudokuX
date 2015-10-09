using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SudokuX.UI.Common.Enums;

namespace SudokuX.UI.Controls
{
    public class BorderToBrushConverter : IValueConverter
    {
        private static Brush special = new LinearGradientBrush(new GradientStopCollection { new GradientStop(Colors.Red, 0.0), new GradientStop(Colors.Green, 1.0) }, 45);
        private static Brush lightBlock = new SolidColorBrush(Color.FromArgb(180, 20, 20, 20));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bt = (BorderType)value;
            switch (bt)
            {
                case BorderType.Block:
                    if (parameter == null)
                        return Brushes.Black;
                    return lightBlock;
                case BorderType.Special:
                    //return Brushes.DarkSlateGray;
                    return special;
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
