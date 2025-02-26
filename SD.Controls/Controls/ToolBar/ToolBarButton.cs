using System.Windows.Input;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using IconPacks.Avalonia.BoxIcons;

namespace SD.Controls.Controls
{
    public class ToolBarButton : ToolBarControl
    {
        public string Id { get; }
        public string Name { get; }
        public PackIconBoxIconsKind IconKind { get; }
        public Control Icon { get; set; }
        public ICommand Command { get; }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }


        public ToolBarButton(string id, string name, PackIconBoxIconsKind iconKind, ICommand command)
        {
            Id = id;
            Name = name;
            IconKind = iconKind,
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
        //public ToolBarButton(string id, string name, string iconKind, ICommand command)
        //{

        //    object? result; 
        //    Enum.TryParse(typeof(PackIconBoxIconsKind), iconKind, out result);
        //    Id = id;
        //    Name = name;
        //    IconKind = result != null ? (PackIconBoxIconsKind)result : PackIconBoxIconsKind.None;
        //    Icon = new PackIconBoxIcons()
        //    {
        //        Kind = result != null ? (PackIconBoxIconsKind)result : PackIconBoxIconsKind.None,
        //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
        //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
        //        Height = 24,
        //        Width = 24
        //    };
        //    Command = command;
        //}
    }

}
