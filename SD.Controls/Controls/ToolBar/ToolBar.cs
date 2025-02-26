using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using System.Windows.Input;
using Avalonia.Controls.Primitives;

namespace SD.Controls.Controls
{
    public partial class ToolBar : TemplatedControl
    {
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            ToolBarService.Instance.Controls.CollectionChanged += (_, __) =>
            {
                // this.InvalidateVisual();  // 🔄 UI aktualisieren, wenn sich Buttons ändern
            };
        }

       

    private void ToolBar_DataContextChanged(object? sender, EventArgs e)
        {
           
        }

        #region Dependency Properties
        public static readonly StyledProperty<ObservableCollection<ToolBarControl>> ControlsProperty =
    AvaloniaProperty.Register<ToolBar, ObservableCollection<ToolBarControl>>(nameof(Controls), ToolBarService.Instance.Controls);

        public ObservableCollection<ToolBarControl> Controls
        {
            get => GetValue(ControlsProperty);
            set => SetValue(ControlsProperty, value);
        }
        #endregion
    }
}
