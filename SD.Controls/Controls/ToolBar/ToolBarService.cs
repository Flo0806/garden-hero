
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using IconPacks.Avalonia.BoxIcons;
using System.Xml.Linq;

namespace SD.Controls.Controls
{
    public class ToolBarService : ObservableObject, IToolBarService
    {
        private static ToolBarService? _instance;
        public static ToolBarService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ToolBarService();
                }
                return _instance;
            }
        }
        public ObservableCollection<ToolBarControl> Controls { get; } = new();

        public void RegisterButton(string id, string name, PackIconBoxIconsKind iconKind, ICommand command)
        {
            if (Controls.Any(b => b is ToolBarButton tbb && tbb.Id == id)) return; // Keine Duplikate
            Controls.Add(new ToolBarButton(id, name, iconKind, command));
        }

        public void RemoveButton(string id)
        {
            var button = Controls.FirstOrDefault(b => b is ToolBarButton tbb && tbb.Id == id);
            if (button != null)
                Controls.Remove(button);
        }

        public void RegisterSeparator(string id)
        {
            if (Controls.Any(b => b is ToolBarSeparator tbs && tbs.Id == id)) return; // Keine Duplikate
            Controls.Add(new ToolBarSeparator(id));
        }

        public void RemoveSeparator(string id)
        {
            var separator = Controls.FirstOrDefault(b => b is ToolBarSeparator tbs && tbs.Id == id);
            if (separator != null)
                Controls.Remove(separator);
        }

        public void ClearControls()
        {
            Controls.Clear();
        }
    }

}
