using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;

namespace SD.Controls.Controls
{
    public class RenderEvent : AvaloniaObject
    {
        public Guid Id { get; set; }
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GridRow { get; set; }
        public int GridColumn { get; set; }
        public int ColumnSpan { get; set; }
        public string ColorKey { get; set; }

        public Thickness CellMargin { get; set; } = new Thickness(0);

        // 🔹 IsSelected Property mit PropertyChanged-Update
        public static readonly StyledProperty<bool> IsSelectedProperty =
            AvaloniaProperty.Register<RenderEvent, bool>(nameof(IsSelected), false);

        public bool IsSelected
        {
            get => GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }
    }

}
