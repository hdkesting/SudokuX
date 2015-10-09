using SudokuX.UI.Common.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SudokuX.UI.Controls
{
    public class HighlightToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hl = (Highlight)value;

            if (hl == Highlight.None)
                return 0.0; // transparent = no highlight

            double opacity = 0.0; // 0 = transparent, 1 = blocking

            if ((hl & Highlight.Easy) != 0)
            {
                opacity = 0.2;
            }
            else if ((hl & Highlight.Group) != 0)
            {
                opacity = 0.9;
            }
            /*else if ((hl & Highlight.Pencil) != 0)
            {
                opacity = 0.35;
            }*/
            else if ((hl & Highlight.Pen) != 0)
            {
                opacity = 0.8;
            }

            return opacity;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
