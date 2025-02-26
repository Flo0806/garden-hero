using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenHero.Models
{
    public class CalendarEvent
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty; // Titel der Erinnerung
        public string Description { get; set; } = string.Empty; // Beschreibung (optional)

        public DateTime StartDate { get; set; } // Startdatum
        public DateTime EndDate { get; set; } // Enddatum

        public string Tags { get; set; } = string.Empty; // Komma-separierte Tags für Filterung

        public string ColorKey { get; set; } = "DefaultEventColor"; // Referenz zu SolidColorBrush x:Key

        public bool IsManual { get; set; } = false; // Manuell oder automatisch erzeugt?

        // Hilfsmethode für einzelne Tages-Events
        public bool IsSingleDay => StartDate.Date == EndDate.Date;
    }

}
