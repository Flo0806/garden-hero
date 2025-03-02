using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SD.Controls.Controls;


namespace GardenHero.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
//        private readonly IToolBarService _toolBarService;

        public string Greeting { get; } = "Welcome to Avalonia!";
        [ObservableProperty]
        private ViewModelBase _currentView;

        [ObservableProperty]
        private ToolBarService _toolBarService;

        [ObservableProperty]
        private int[] _arrayData;

        [ObservableProperty]
        private ObservableCollection<CalendarEvent> _events = new ObservableCollection<CalendarEvent>();

        [ObservableProperty]
        private DateTime _currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1);

        partial void OnArrayDataChanged(int[]? oldValue, int[] newValue)
        {
            
        }

        public MainWindowViewModel()
        {
            CurrentView = new CategoryViewModel("Ein Test");

            ToolBarService = ToolBarService.Instance;

            // Toolbar-Button registrieren
            ToolBarService.RegisterButton("save", "Speichern", IconPacks.Avalonia.BoxIcons.PackIconBoxIconsKind.SolidFilePlus , SaveCommand);
            ToolBarService.RegisterSeparator("first");
            DateTime CurrentMonth = DateTime.Now;

            Events = new ObservableCollection<CalendarEvent>();

            Events.Add(new CalendarEvent { Title = "Tomaten säen", ColorKey = "EventRedBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month - 1, 20), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 10) });
            Events.Add(new CalendarEvent { Title = "Gurken säen", ColorKey = "EventBlueBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 6), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 8) });
            Events.Add(new CalendarEvent { Title = "Birnen säen", ColorKey = "EventVioletBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month -1, 22), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month + 1, 7) });
            Events.Add(new CalendarEvent { Title = "Äpfel säen", ColorKey = "EventGreenBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 9), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 9) });


            //Task.Run(async () =>
            //{
            //    await Task.Delay(3000);
            //    Dispatcher.UIThread.Invoke(() =>
            //    {
            //        Events.Add(new CalendarEvent { Title = "Tomaten säen", ColorKey = "EventRedBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 5), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 10) });
            //        Events.Add(new CalendarEvent { Title = "Gurken säen", ColorKey = "EventBlueBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 6), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 8) });
            //        Events.Add(new CalendarEvent { Title = "Birnen säen", ColorKey = "EventVioletBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 8), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 10) });
            //        Events.Add(new CalendarEvent { Title = "Äpfel säen", ColorKey = "EventGreenBrush", StartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 9), EndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 9) });

            //    });
            //});

        }

        ~MainWindowViewModel()
        {

        }


        [RelayCommand(CanExecute = nameof(SaveCommandCanExecute))]
        private void Save()
        {
            // Speichern-Logik
        }

        private bool SaveCommandCanExecute() => true;
    }
}
