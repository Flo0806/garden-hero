using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using IconPacks.Avalonia.BoxIcons;

namespace SD.Controls.Controls
{
    public class ToolBarButton : ToolBarControl
    {
        public string Name { get; }
        public Control Icon { get; set; }
        public ICommand Command { get; }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }


        public ToolBarButton(string id, string name, PackIconBoxIconsKind iconKind, ICommand command) : base(id)
        {
            Name = name;
            Icon = new PackIconBoxIcons()
            {
                Kind = iconKind,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                Height = 24,
                Width = 24
            };
            Command = command;
        }
    }

}
