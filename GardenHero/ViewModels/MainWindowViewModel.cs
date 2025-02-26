using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        public MainWindowViewModel()
        {
            CurrentView = new CategoryViewModel("Ein Test");

            ToolBarService = ToolBarService.Instance;

            // Toolbar-Button registrieren
            ToolBarService.RegisterButton("save", "Speichern", IconPacks.Avalonia.BoxIcons.PackIconBoxIconsKind.SolidFilePlus , SaveCommand);
            ToolBarService.RegisterSeparator("first");
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
