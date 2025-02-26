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
    public class RowToPixelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int rowIndex && parameter is Control calendarControl)
            {
                if (calendarControl is Grid grid && grid.RowDefinitions.Count > 0)
                {
                    double cellHeight = grid.Bounds.Height / 5; // 5 Zeilen für die Wochen des Monats
                    return rowIndex * cellHeight;
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
