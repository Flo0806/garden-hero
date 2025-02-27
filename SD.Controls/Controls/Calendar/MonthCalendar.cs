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
using Avalonia.Markup.Xaml.Templates;

namespace SD.Controls.Controls
{
    [TemplatePart("PART_PreviousButton", typeof(Button))]
    [TemplatePart("PART_NextButton", typeof(Button))]
    public partial class MonthCalendar : TemplatedControl
    {
        protected Button? PreviousButtonPart { get; private set; }
        protected Button? NextButtonPart { get; private set; }
        protected Grid? CalendarGridPart { get; private set; }

        static MonthCalendar()
        {
            AffectsRender<MonthCalendar>(CurrentMonthProperty);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            PreviousButtonPart = e.NameScope.Get<Button>("PART_PreviousButton");
            NextButtonPart = e.NameScope.Get<Button>("PART_NextButton");
            // var itemsControl = e.NameScope.Get<ItemsControl>("PART_ItemsControl");
            CalendarGridPart = e.NameScope.Get<Grid>("PART_Grid");
           
           //itemsControl.Loaded += ItemsControl_Loaded;
           
            if (PreviousButtonPart == null || NextButtonPart == null)
            {
                throw new ArgumentNullException("Cannot find navigation buttons");
            }

            PreviousButtonPart!.Click += PreviousButton_Click;
            NextButtonPart.Click += NextButton_Click;

            UpdateCalendar();
        }

        private void ItemsControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var test = (sender as ItemsControl).Presenter.Panel;
            BuildCalendarGrid(test as Grid);
        }


        #region Events
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
        #endregion

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

        public static readonly StyledProperty<ObservableCollection<RenderEvent>> RenderEventsProperty =
           AvaloniaProperty.Register<MonthSelection, ObservableCollection<RenderEvent>>(nameof(RenderEvents), new ObservableCollection<RenderEvent>());

        public ObservableCollection<RenderEvent> RenderEvents
        {
            get => GetValue(RenderEventsProperty);
            set => SetValue(RenderEventsProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<CalendarEvent>> EventsProperty =
    AvaloniaProperty.Register<MonthCalendar, ObservableCollection<CalendarEvent>>(nameof(Events), new ObservableCollection<CalendarEvent>());

        public ObservableCollection<CalendarEvent> Events
        {
            get => GetValue(EventsProperty);
            set => SetValue(EventsProperty, value);
        }

        #endregion

        #region Helper

        private void BuildCalendarGrid(Grid calendarGrid)
        {
            if (calendarGrid == null) return;

            calendarGrid.Children.Clear();

            // 🔹 DataTemplates abrufen
            var dayTemplate = this.FindResource("DayTemplate") as DataTemplate; // (DataTemplate)Application.Current.Resources["DayTemplate"];
            var eventTemplate = this.FindResource("EventTemplate") as DataTemplate; // (DataTemplate)Application.Current.Resources["EventTemplate"];

            // 🔹 Tage ins Grid setzen
            foreach (var day in Days)
            {
                var dayControl = new ContentControl
                {
                    Content = day,
                    ContentTemplate = dayTemplate
                };

                Grid.SetColumn(dayControl, day.GridColumn);
                Grid.SetRow(dayControl, day.GridRow);

                calendarGrid.Children.Add(dayControl);
            }

            // 🔹 Events ins Grid setzen
            foreach (var evt in RenderEvents)
            {
                var eventControl = new EventControl
                {
                    Title = evt.Title,
                    Background = this.FindResource(evt.ColorKey) is SolidColorBrush solidColorBrush
                ? solidColorBrush
                : Brushes.Transparent
                };

                Grid.SetColumn(eventControl, evt.GridColumn);
                Grid.SetRow(eventControl, evt.GridRow);
                Grid.SetColumnSpan(eventControl, evt.ColumnSpan);

                calendarGrid.Children.Add(eventControl);
            }
        }


        private int GetGridRow(DateTime date)
        {
            int firstDayOfMonth = (int)new DateTime(date.Year, date.Month, 1).DayOfWeek;
            firstDayOfMonth = firstDayOfMonth == 0 ? 6 : firstDayOfMonth - 1; // Sonntag ist 6
            return (date.Day + firstDayOfMonth - 1) / 7;
        }

        #endregion

        private void UpdateRenderEvents()
        {
            RenderEvents.Clear();
            var tempRenderEvents = new List<RenderEvent>();

            foreach (var evt in Events)
            {
                DateTime tempStart = evt.StartDate;
                while (tempStart <= evt.EndDate)
                {
                    int weekRow = GetGridRow(tempStart); // Berechnung der Zeile (Woche)
                    int startColumn = (int)tempStart.DayOfWeek == 0 ? 6 : (int)tempStart.DayOfWeek - 1;
                    DateTime weekEnd = tempStart.AddDays(6 - startColumn);

                    if (weekEnd > evt.EndDate)
                        weekEnd = evt.EndDate;

                    int columnSpan = (weekEnd - tempStart).Days + 1;

                    tempRenderEvents.Add(new RenderEvent
                    {
                        Title = evt.Title,
                        StartDate = tempStart,
                        EndDate = weekEnd,
                        GridRow = weekRow,
                        GridColumn = startColumn,
                        ColumnSpan = columnSpan,
                        ColorKey = evt.ColorKey
                    });

                    tempStart = weekEnd.AddDays(1);
                }
            }
        
            // 🔹 Schritt 2: Events pro Woche sortieren (Priorität: Früher → Länger → Kürzer)
            var groupedEvents = tempRenderEvents.GroupBy(e => e.GridRow)
                .ToDictionary(g => g.Key, g => g.OrderBy(e => e.StartDate).ThenByDescending(e => e.ColumnSpan).ToList());

            // 🔹 Schritt 3: Maximal 3 Events pro Woche anzeigen
            foreach (var week in groupedEvents)
            {
                int visibleCount = 0;

                foreach (var renderEvent in week.Value)
                {
                    if (visibleCount < 3)
                    {
                        RenderEvents.Add(renderEvent);
                        visibleCount++;
                    }
                    else
                    {
                        // Falls mehr als 3 Events → "Mehr anzeigen" setzen (später für Button-Logik)
                        //MoreEventsIndicator[week.Key] = true;
                        break;
                    }
                }
            }
        }


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

            int rowIndex = 0;
            int columnIndex = 0;

            // Test-Events für den aktuellen Monat hinzufügen (inklusive mehrtägiger Events)
            Events.Clear();
            Events.Add(new CalendarEvent { Title = "Tomaten säen", ColorKey = "RedBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 5), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 10) });

            // Alle Events für den aktuellen Monat holen
            var eventsInMonth = Events.Where(e => e.StartDate.Month == CurrentMonth.Month || e.EndDate.Month == CurrentMonth.Month).ToList();

            // 🌟 Vormonat-Tage auffüllen
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
                    Events = eventsInMonth.Where(e => e.StartDate.Date <= dayDate.Date && e.EndDate.Date >= dayDate.Date).ToList(),
                    GridColumn = columnIndex,
                    GridRow = rowIndex
                });

                columnIndex++;
            }

            // 🌟 Aktueller Monat
            for (int day = 1; day <= daysInMonth; day++)
            {
                if (columnIndex == 7)
                {
                    columnIndex = 0;
                    rowIndex++;
                }

                DateTime dayDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, day);
                var dayEvents = eventsInMonth.Where(e => e.StartDate.Date <= dayDate.Date && e.EndDate.Date >= dayDate.Date).ToList();

                Days.Add(new CalendarDay
                {
                    DayNumber = day,
                    StartDate = dayDate,
                    IsCurrentMonth = true,
                    Events = dayEvents,
                    GridColumn = columnIndex,
                    GridRow = rowIndex
                });

                columnIndex++;
            }

            // 🌟 Nächster Monat auffüllen
            for (int i = 1; Days.Count < 35; i++)
            {
                if (columnIndex == 7)
                {
                    columnIndex = 0;
                    rowIndex++;
                }

                DateTime dayDate = new DateTime(firstDayOfMonth.AddMonths(1).Year, firstDayOfMonth.AddMonths(1).Month, i);

                Days.Add(new CalendarDay
                {
                    DayNumber = i,
                    StartDate = dayDate,
                    IsCurrentMonth = false,
                    Events = eventsInMonth.Where(e => e.StartDate.Date <= dayDate.Date && e.EndDate.Date >= dayDate.Date).ToList(),
                    GridColumn = columnIndex,
                    GridRow = rowIndex
                });

                columnIndex++;
            }

            UpdateRenderEvents();

            BuildCalendarGrid(CalendarGridPart);
        }





       

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            return;
            //if (Days == null || Days.Count == 0) return;

            //foreach (var evt in Events)
            //{
            //    int startColumn = evt.StartColumn;
            //    int startRow = evt.StartRow;
            //    int duration = evt.Duration;

            //    double cellWidth = Bounds.Width / 7;  // 7 Spalten
            //    double cellHeight = Bounds.Height / 5; // 5 Zeilen

            //    double x = startColumn * cellWidth;
            //    double y = startRow * cellHeight + 80;
            //    double width = duration * cellWidth;

            //    var rect = new Rect(x, y, width, 10);
            //    var brush = new SolidColorBrush(Avalonia.Media.Colors.Blue);

            //    context.DrawRectangle(brush, null, rect, 5);
            //}
        }
    }
}

