using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Controls.Controls
{
    public class CalendarEvent
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ColorKey { get; set; } = "DefaultEventColor";

        public int Duration => (EndDate - StartDate).Days + 1; // Anzahl der Tage

        // Startposition im Grid
        public int StartColumn => (int)StartDate.DayOfWeek == 0 ? 6 : (int)StartDate.DayOfWeek - 1;
        public int StartRow => (StartDate.Day - 1) / 7;
    }


}
