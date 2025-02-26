using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Controls.Common.Converters
{
    public class ColumnToPixelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int columnIndex && parameter is Control calendarControl)
            {
                if (calendarControl is Grid grid && grid.ColumnDefinitions.Count > 0)
                {
                    double cellWidth = grid.Bounds.Width / 7; // 7 Spalten für die Tage der Woche
                    return columnIndex * cellWidth;
                }
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
