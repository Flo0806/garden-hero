using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using IconPacks.Avalonia.BoxIcons;

namespace SD.Controls.Controls
{
    public interface IToolBarService
    {
        ObservableCollection<ToolBarControl> Controls { get; }

        void RegisterButton(string id, string name, PackIconBoxIconsKind iconKind, ICommand command);
        void RemoveButton(string id);

        void RegisterSeparator(string id);
        void RemoveSeparator(string id);
        void ClearControls();
    }

    public partial class ToolBarControl: ObservableObject
    {
        public string Id { get; }

        public ToolBarControl(string id)
        {
            Id = id;
        }
    }

}
