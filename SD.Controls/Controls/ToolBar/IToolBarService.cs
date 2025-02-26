using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SD.Controls.Controls
{
    public interface IToolBarService
    {
        ObservableCollection<ToolBarControl> Controls { get; }

        void RegisterButton(string id, string name, string iconKind, ICommand command);
        void RemoveButton(string id);
        void ClearButtons();
    }

    public partial class ToolBarControl: ObservableObject
    {

    }

}
