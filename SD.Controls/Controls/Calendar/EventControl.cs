using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia;

namespace SD.Controls.Controls
{
    public class EventControl : TemplatedControl
    {
        // Styled Property für den Titel des Events
        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<EventControl, string>(nameof(Title), "Neues Event");

        public string Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // Styled Property für den Hintergrund des Events
        public static new readonly StyledProperty<IBrush> BackgroundProperty =
            AvaloniaProperty.Register<EventControl, IBrush>(nameof(Background), Brushes.Gray);

        public IBrush Background
        {
            get => GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }
    }
}
