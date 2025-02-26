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
    public class DayToPixelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var test = parameter;
            if (value is int days && parameter is Control calendarControl)
            {
                if (calendarControl is Grid grid && grid.ColumnDefinitions.Count > 0)
                {
                    double cellWidth = grid.Bounds.Width / 7; // 7 Spalten für die Tage der Woche
                    return days * cellWidth;
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
