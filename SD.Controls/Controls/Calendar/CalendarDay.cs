using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Controls.Controls
{
    public class CalendarDay
    {
        public int DayNumber { get; set; }
        public string DayName => StartDate.ToString("ddd");
        public DateTime StartDate { get; set; }
        public bool IsCurrentMonth { get; set; }

        public List<CalendarEvent> Events { get; set; } = new(); // Enthält alle Events für diesen Tag

        public bool HasMoreEvents => Events.Count > 3; // Falls mehr als 3 Events existieren
    }


}
