using Avalonia;
using Avalonia.Controls.Metadata;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SD.Controls.Controls;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SD.Controls.Controls
{
    [TemplatePart("PART_PreviousButton", typeof(Button))]
    [TemplatePart("PART_NextButton", typeof(Button))]
    public partial class MonthCalendar : TemplatedControl
    {
        protected Button? PreviousButtonPart { get; private set; }
        protected Button? NextButtonPart { get; private set; }

        static MonthCalendar()
        {
            AffectsRender<MonthCalendar>(CurrentMonthProperty);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            PreviousButtonPart = e.NameScope.Get<Button>("PART_PreviousButton");
            NextButtonPart = e.NameScope.Get<Button>("PART_NextButton");

            if (PreviousButtonPart == null || NextButtonPart == null)
            {
                throw new ArgumentNullException("Cannot find navigation buttons");
            }

            PreviousButtonPart!.Click += PreviousButton_Click;
            NextButtonPart.Click += NextButton_Click;

            UpdateCalendar();
        }

        private void NextButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
            UpdateCalendar();
        }

        private void PreviousButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
            UpdateCalendar();
        }

        #region Dependency Properties
        public static readonly StyledProperty<DateTime> CurrentMonthProperty =
            AvaloniaProperty.Register<MonthCalendar, DateTime>(nameof(CurrentMonth), defaultValue: DateTime.Today);

        public DateTime CurrentMonth
        {
            get => GetValue(CurrentMonthProperty);
            set => SetValue(CurrentMonthProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<CalendarDay>> DaysProperty =
            AvaloniaProperty.Register<MonthSelection, ObservableCollection<CalendarDay>>(nameof(Days), new ObservableCollection<CalendarDay>());

        public ObservableCollection<CalendarDay> Days
        {
            get => GetValue(DaysProperty);
            set => SetValue(DaysProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<CalendarEvent>> EventsProperty =
    AvaloniaProperty.Register<MonthCalendar, ObservableCollection<CalendarEvent>>(nameof(Events), new ObservableCollection<CalendarEvent>());

        public ObservableCollection<CalendarEvent> Events
        {
            get => GetValue(EventsProperty);
            set => SetValue(EventsProperty, value);
        }

        #endregion

        /// <summary>
        /// Berechnet die Tage des Monats und füllt die ObservableCollection.
        /// </summary>
        private void UpdateCalendar()
        {
            Days.Clear();
            DateTime firstDayOfMonth = new(CurrentMonth.Year, CurrentMonth.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month);
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            startDayOfWeek = startDayOfWeek == 0 ? 7 : startDayOfWeek; // Sonntag als 7

            // Test-Events für den aktuellen Monat hinzufügen (inklusive mehrtägiger Events)
            Events.Clear();
            Events.Add(new CalendarEvent { Title = "Tomaten säen", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 5), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 10) });
           // Events.Add(new CalendarEvent { Title = "Ernte Salat", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 12), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 12) });
          //  Events.Add(new CalendarEvent { Title = "Düngen", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 20), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 22) });

            // Hole alle Events für den aktuellen Monat
            var eventsInMonth = Events.Where(e => e.StartDate.Month == CurrentMonth.Month || e.EndDate.Month == CurrentMonth.Month).ToList();

            // Vormonat-Tage auffüllen
            DateTime prevMonth = firstDayOfMonth.AddMonths(-1);
            int prevMonthDays = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
            for (int i = startDayOfWeek - 1; i > 0; i--)
            {
                DateTime dayDate = new DateTime(prevMonth.Year, prevMonth.Month, prevMonthDays - i + 1);
                Days.Add(new CalendarDay
                {
                    DayNumber = dayDate.Day,
                    StartDate = dayDate,
                    IsCurrentMonth = false,
                    Events = eventsInMonth.Where(e => e.StartDate.Date <= dayDate.Date && e.EndDate.Date >= dayDate.Date).ToList()
                });
            }

            // Aktueller Monat
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime dayDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, day);
                var dayEvents = eventsInMonth.Where(e => e.StartDate.Date <= dayDate.Date && e.EndDate.Date >= dayDate.Date).ToList();

                Days.Add(new CalendarDay
                {
                    DayNumber = day,
                    StartDate = dayDate,
                    IsCurrentMonth = true,
                    Events = dayEvents
                });
            }

            // Nächster Monat auffüllen
            int totalDays = Days.Count;
            DateTime nextMonth = firstDayOfMonth.AddMonths(1);
            for (int i = 1; totalDays < 35; i++, totalDays++)
            {
                DateTime dayDate = new DateTime(nextMonth.Year, nextMonth.Month, i);
                Days.Add(new CalendarDay
                {
                    DayNumber = i,
                    StartDate = dayDate,
                    IsCurrentMonth = false,
                    Events = eventsInMonth.Where(e => e.StartDate.Date <= dayDate.Date && e.EndDate.Date >= dayDate.Date).ToList()
                });
            }
        }


        //private void UpdateCalendar()
        //{
        //    Days.Clear();
        //    DateTime firstDayOfMonth = new(CurrentMonth.Year, CurrentMonth.Month, 1);
        //    int daysInMonth = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month);
        //    int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
        //    startDayOfWeek = startDayOfWeek == 0 ? 7 : startDayOfWeek; // Sonntag als 7

        //    // Vormonat-Tage auffüllen (falls nötig)
        //    DateTime prevMonth = firstDayOfMonth.AddMonths(-1);
        //    int prevMonthDays = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
        //    for (int i = startDayOfWeek - 1; i > 0; i--)
        //    {
        //        Days.Add(new CalendarDay
        //        {
        //            DayNumber = prevMonthDays - i + 1,
        //            StartDate = new DateTime(prevMonth.Year, prevMonth.Month, prevMonthDays - i + 1),
        //            IsCurrentMonth = false
        //        });
        //    }

        //    // Aktueller Monat
        //    for (int day = 1; day <= daysInMonth; day++)
        //    {
        //        Days.Add(new CalendarDay
        //        {
        //            DayNumber = day,
        //            StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, day),
        //            IsCurrentMonth = true
        //        });
        //    }

        //    // Nächster Monat auffüllen (falls Platz in der 7x5-Grid-Struktur ist)
        //    int totalDays = Days.Count;
        //    DateTime nextMonth = firstDayOfMonth.AddMonths(1);
        //    for (int i = 1; totalDays < 35; i++, totalDays++)
        //    {
        //        Days.Add(new CalendarDay
        //        {
        //            DayNumber = i,
        //            StartDate = new DateTime(nextMonth.Year, nextMonth.Month, i),
        //            IsCurrentMonth = false
        //        });
        //    }
        //}

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            return;
            if (Days == null || Days.Count == 0) return;

            foreach (var evt in Events)
            {
                int startColumn = evt.StartColumn;
                int startRow = evt.StartRow;
                int duration = evt.Duration;

                double cellWidth = Bounds.Width / 7;  // 7 Spalten
                double cellHeight = Bounds.Height / 5; // 5 Zeilen

                double x = startColumn * cellWidth;
                double y = startRow * cellHeight + 80;
                double width = duration * cellWidth;

                var rect = new Rect(x, y, width, 10);
                var brush = new SolidColorBrush(Avalonia.Media.Colors.Blue);

                context.DrawRectangle(brush, null, rect, 5);
            }
        }
    }
}

