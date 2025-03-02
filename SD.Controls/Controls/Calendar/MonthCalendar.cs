using Avalonia;
using Avalonia.Controls.Metadata;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using System.Collections.ObjectModel;
using Avalonia.Markup.Xaml.Templates;
using System.ComponentModel;

namespace SD.Controls.Controls
{
    [TemplatePart("PART_PreviousButton", typeof(Button))]
    [TemplatePart("PART_NextButton", typeof(Button))]
    [TemplatePart("PART_Grid", typeof(Grid))]
    public partial class MonthCalendar : TemplatedControl
    {
        protected Button? PreviousButtonPart { get; private set; }
        protected Button? NextButtonPart { get; private set; }
        protected Grid? CalendarGridPart { get; private set; }

        static MonthCalendar()
        {
            CurrentMonthProperty.Changed.AddClassHandler<MonthCalendar>((x, e) =>
            {
                if (e.NewValue != null && e.NewValue is DateTime newMonthDate)
                {
                    x.CurrentMonthText = newMonthDate.ToString("MMMM yyyy");
                }
            });

            EventsProperty.Changed.AddClassHandler<MonthCalendar>((x, e) =>
            {
                x.Events.CollectionChanged += (sender, e) =>
                {
                    if (e.NewItems != null)
                    {
                        foreach (CalendarEvent evt in e.NewItems)
                        {
                            evt.PropertyChanged += x.CalendarEventChanged;
                        }
                    }

                    if (e.OldItems != null)
                    {
                        foreach (CalendarEvent evt in e.OldItems)
                        {
                            evt.PropertyChanged -= x.CalendarEventChanged;
                        }
                    }

                    x.UpdateRenderEvents(); // Falls ein Event hinzugefügt/entfernt wurde, RenderEvents neu berechnen
                    x.RenderEventsToCalendar();
                };

                foreach (var evt in x.Events)
                {
                    evt.PropertyChanged += x.CalendarEventChanged;
                }

                x.UpdateRenderEvents();
                x.RenderEventsToCalendar();
            });
            AffectsRender<MonthCalendar>(CurrentMonthProperty);
        }

        public MonthCalendar()
        {
        }

        private void CalendarEventChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is CalendarEvent evt)
            {
                Console.WriteLine($"Event geändert: {evt.Title}, Property: {e.PropertyName}");
                UpdateRenderEvents(); // RenderEvents neu berechnen
                RenderEventsToCalendar();
            }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            PreviousButtonPart = e.NameScope.Get<Button>("PART_PreviousButton");
            NextButtonPart = e.NameScope.Get<Button>("PART_NextButton");

            CalendarGridPart = e.NameScope.Get<Grid>("PART_Grid");

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
            RenderDaysToCalendar();
            RenderEventsToCalendar();
        }

        #region Custom Events
        public event EventHandler<RenderEvent?>? EventSelectionChanged;

        public event EventHandler<DateTime>? CurrentMonthChanged;

        public event EventHandler<CalendarEvent>? CalendarEventDoubleClicked;
        #endregion

        #region Events
        private void NextButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
            CurrentMonthChanged?.Invoke(this, CurrentMonth);
            UpdateCalendar();
        }

        private void PreviousButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
            CurrentMonthChanged?.Invoke(this, CurrentMonth);
            UpdateCalendar();
        }
        #endregion

        #region Dependency Properties
        // CurrentMonthText
        public static readonly StyledProperty<string> CurrentMonthTextProperty =
    AvaloniaProperty.Register<MonthCalendar, string>(nameof(CurrentMonthText), DateTime.Now.ToString("MMMM yyyy"));

        public string CurrentMonthText
        {
            get => GetValue(CurrentMonthTextProperty);
            set => SetValue(CurrentMonthTextProperty, value);
        }

        public static readonly StyledProperty<DateTime> CurrentMonthProperty =
            AvaloniaProperty.Register<MonthCalendar, DateTime>(nameof(CurrentMonth), defaultValue: DateTime.Today);

        public DateTime CurrentMonth
        {
            get => GetValue(CurrentMonthProperty);
            set => SetValue(CurrentMonthProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<CalendarDay>> DaysProperty =
            AvaloniaProperty.Register<MonthCalendar, ObservableCollection<CalendarDay>>(nameof(Days), new ObservableCollection<CalendarDay>());

        public ObservableCollection<CalendarDay> Days
        {
            get => GetValue(DaysProperty);
            set => SetValue(DaysProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<RenderEvent>> RenderEventsProperty =
           AvaloniaProperty.Register<MonthCalendar, ObservableCollection<RenderEvent>>(nameof(RenderEvents), new ObservableCollection<RenderEvent>());

        public ObservableCollection<RenderEvent> RenderEvents
        {
            get => GetValue(RenderEventsProperty);
            set => SetValue(RenderEventsProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<CalendarEvent>> EventsProperty =
        AvaloniaProperty.Register<MonthCalendar, ObservableCollection<CalendarEvent>>(nameof(Events));

        public ObservableCollection<CalendarEvent> Events
        {
            get => GetValue(EventsProperty);
            set => SetValue(EventsProperty, value);
        }

        #endregion

        #region Helper
        private void RenderDaysToCalendar()
        {
            if (CalendarGridPart == null) return;

            var calendarGrid = CalendarGridPart!;

            // Alle ContentControls mit CalendarDay aus der Children-Collection entfernen
            var daysToRemove = calendarGrid.Children
                .OfType<ContentControl>()
                .Where(cc => cc.Content is CalendarDay)
                .ToList();

            foreach (var dayControl in daysToRemove)
            {
                calendarGrid.Children.Remove(dayControl);
            }

            var dayTemplate = this.FindResource("DayTemplate") as DataTemplate;

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
        }


        private void RenderEventsToCalendar()
        {
            if (CalendarGridPart == null) return;

            var calendarGrid = CalendarGridPart!;

            // Alle EventControls aus der Children-Collection entfernen
            var eventsToRemove = calendarGrid.Children != null ? calendarGrid.Children
                .OfType<EventControl>()
                .ToList() : new List<EventControl>();

            foreach (var eventControl in eventsToRemove)
            {
                calendarGrid.Children!.Remove(eventControl);
            }

            foreach (var evt in RenderEvents)
            {
                var eventControl = new EventControl
                {
                    Title = evt.Title,
                    Background = this.FindResource(evt.ColorKey) is SolidColorBrush solidColorBrush
                        ? solidColorBrush
                        : Brushes.Transparent,
                    DataContext = evt
                };

                Grid.SetColumn(eventControl, evt.GridColumn);
                Grid.SetRow(eventControl, evt.GridRow);
                Grid.SetColumnSpan(eventControl, evt.ColumnSpan);

                // 🔹 EventControls IMMER oben rendern
                eventControl.ZIndex = 2;

                // 🔹 Click-Event → Alle Segmente des Events markieren
                eventControl.PointerPressed += (sender, e) =>
                {
                    bool isSelected = !evt.IsSelected; // Umschalten

                    // Alle Events deselektieren
                    foreach (var instance in RenderEvents)
                    {
                        instance.IsSelected = false;
                    }

                    // Nur geklicktes selektieren
                    foreach (var instance in RenderEvents.Where(x => x.Id == evt.Id))
                    {
                        instance.IsSelected = isSelected;
                    }
                };

                eventControl.DoubleTapped += (sender, e) =>
                {
                    CalendarEvent? currentEvent = Events.Where(ev => ev.Id == evt.EventId).FirstOrDefault();
                    if (currentEvent != null)
                        CalendarEventDoubleClicked?.Invoke(this, currentEvent);
                };

                calendarGrid.Children!.Add(eventControl);
            }
        }


        private int GetGridRow(DateTime date)
        {
            // 🔹 Berechne den ersten sichtbaren Tag (inkl. Vormonats-Tage)
            DateTime firstDayOfMonth = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            firstDayOfWeek = firstDayOfWeek == 0 ? 7 : firstDayOfWeek; // Sonntag als 7

            int visiblePrevMonthDays = firstDayOfWeek - 1;
            DateTime firstVisibleDay = firstDayOfMonth.AddDays(-visiblePrevMonthDays);

            // 🔹 Korrekte Berechnung der Woche anhand des ersten sichtbaren Tages
            return (date - firstVisibleDay).Days / 7;
        }


        private void UpdateRenderEvents()
        {
            RenderEvents.Clear();
            var tempRenderEvents = new List<RenderEvent>();

            // 🔹 Berechnung der sichtbaren Tage (einschließlich Vormonat & Folgemonat)
            DateTime firstDayOfMonth = new(CurrentMonth.Year, CurrentMonth.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(CurrentMonth.Year, CurrentMonth.Month);
            DateTime lastDayOfMonth = firstDayOfMonth.AddDays(daysInMonth - 1);

            // 🔹 Vormonats-Tage berechnen
            DateTime prevMonth = firstDayOfMonth.AddMonths(-1);
            int prevMonthDays = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
            int firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            firstDayOfWeek = firstDayOfWeek == 0 ? 7 : firstDayOfWeek; // Sonntag als 7

            int visiblePrevMonthDays = firstDayOfWeek - 1;
            DateTime firstVisibleDay = firstDayOfMonth.AddDays(-visiblePrevMonthDays);

            // 🔹 Folgemonats-Tage berechnen
            int totalVisibleDays = visiblePrevMonthDays + daysInMonth;
            int remainingDays = (totalVisibleDays % 7 == 0) ? 0 : 7 - (totalVisibleDays % 7);
            DateTime lastVisibleDay = lastDayOfMonth.AddDays(remainingDays);

            // 🔹 Alle relevanten Events filtern
            foreach (var evt in Events)
            {
                // 🔹 Nur Events anzeigen, die im sichtbaren Bereich sind
                if (evt.EndDate < firstVisibleDay || evt.StartDate > lastVisibleDay)
                    continue;

                Guid eventId = Guid.NewGuid(); // 🔹 Eine GUID für alle Segmente des Events

                // 🔹 Startzeitpunkt begrenzen
                DateTime tempStart = evt.StartDate < firstVisibleDay ? firstVisibleDay : evt.StartDate;
                if (tempStart > lastVisibleDay) continue; // Falls Start außerhalb des sichtbaren Bereichs ist, überspringen

                while (tempStart <= evt.EndDate)
                {
                    int weekRow = GetGridRow(tempStart); // Zeile berechnen (Woche)
                    int startColumn = (int)tempStart.DayOfWeek == 0 ? 6 : (int)tempStart.DayOfWeek - 1;
                    DateTime weekEnd = tempStart.AddDays(6 - startColumn);

                    if (weekEnd > evt.EndDate)
                        weekEnd = evt.EndDate;

                    if (weekEnd > lastVisibleDay)
                        weekEnd = lastVisibleDay; // Begrenzung auf den sichtbaren Bereich

                    // **Fix: Falls tempStart bereits nach lastVisibleDay liegt, Schleife beenden**
                    if (tempStart > lastVisibleDay)
                        break;

                    int columnSpan = (weekEnd - tempStart).Days + 1;

                    tempRenderEvents.Add(new RenderEvent
                    {
                        Id = eventId, // 🔹 Jedes Segment bekommt die gleiche ID!
                        EventId = evt.Id, // Verknüpft mit dem Event
                        Title = evt.Title,
                        StartDate = tempStart,
                        EndDate = weekEnd,
                        GridRow = weekRow,
                        GridColumn = startColumn,
                        ColumnSpan = columnSpan,
                        ColorKey = evt.ColorKey
                    });

                    tempStart = weekEnd.AddDays(1);

                    // **Fix: Falls tempStart bereits nach lastVisibleDay liegt, Schleife beenden**
                    if (tempStart > lastVisibleDay)
                        break;
                }
            }

            // 🔹 Events pro Woche sortieren (Priorität: Früher → Länger → Kürzer)
            var groupedEvents = tempRenderEvents.GroupBy(e => e.GridRow)
                .ToDictionary(g => g.Key, g => g.OrderBy(e => e.StartDate).ThenByDescending(e => e.ColumnSpan).ToList());

            // 🔹 Maximale Anzahl pro Zeile setzen (z. B. max 3)
            const int maxEventsPerRow = 3;
            const int marginStep = 25;
            const int firstMarginHeight = 25; // Allererster Abstand

            var finalRenderEvents = new ObservableCollection<RenderEvent>();

            foreach (var weekRow in groupedEvents.Keys)
            {
                var placedEvents = new List<RenderEvent>();

                foreach (var evt in groupedEvents[weekRow])
                {
                    // Prüfen, ob Platz in der Zeile ist
                    if (placedEvents.Count < maxEventsPerRow)
                    {
                        evt.CellMargin = new Thickness(0, placedEvents.Count * marginStep + firstMarginHeight, 0, 0);
                        placedEvents.Add(evt);
                        finalRenderEvents.Add(evt);
                    }
                    else
                    {
                        // Sobald ein Event nicht platziert werden kann, bleibt es komplett unsichtbar
                        break;
                    }
                }
            }

            // 🔹 finalRenderEvents enthält jetzt nur Events, die wirklich angezeigt werden
            RenderEvents = finalRenderEvents;
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

            //Events.Add(new CalendarEvent { Title = "Tomaten säen", ColorKey = "EventRedBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 5), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 10) });
            //Events.Add(new CalendarEvent { Title = "Gurken säen", ColorKey = "EventBlueBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 6), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 8) });
            //Events.Add(new CalendarEvent { Title = "Birnen säen", ColorKey = "EventVioletBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 8), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 10) });
            //Events.Add(new CalendarEvent { Title = "Äpfel säen", ColorKey = "EventGreenBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 9), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 9) });

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
            for (int i = 1; Days.Count < 42; i++)
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

            //BuildCalendarGrid(CalendarGridPart!);
            RenderDaysToCalendar();
            RenderEventsToCalendar();
        }
        #endregion
    }
}

