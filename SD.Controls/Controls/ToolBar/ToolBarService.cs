
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using IconPacks.Avalonia.BoxIcons;

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

        //public void RegisterButton(string id, string name, string iconKind, ICommand command)
        //{
        //    if (Controls.Any(b => b is ToolBarButton tbb && tbb.Id == id)) return; // Keine Duplikate
        //    Controls.Add(new ToolBarButton(id, name, iconKind, command));
        //}

        public void RemoveButton(string id)
        {
            var button = Controls.FirstOrDefault(b => b is ToolBarButton tbb && tbb.Id == id);
            if (button != null)
                Controls.Remove(button);
        }

        public void RegisterSeparator()
        {
            Controls.Add(new ToolBarSeparator());
        }

        public void ClearButtons()
        {
            Controls.Clear();
        }
    }

}
