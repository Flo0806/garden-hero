using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia;
using Avalonia.Controls;

namespace SD.Controls.Controls
{
    public class EventControl : TemplatedControl
    {
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            PointerPressed += EventControl_PointerPressed;
        }

        private void EventControl_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            //IsSelected = !IsSelected;
        }

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

        // 🔹 Styled Property für "Ausgewählt"-Zustand
        public static readonly StyledProperty<bool> IsSelectedProperty =
            AvaloniaProperty.Register<EventControl, bool>(nameof(IsSelected), true);

        public bool IsSelected
        {
            get => GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        // 🔹 Pseudo-Class für die Selektion registrieren
        static EventControl()
        {
            IsSelectedProperty.Changed.AddClassHandler<EventControl>((x, e) =>
            {
                x.PseudoClasses.Set(":selected", (bool)e.NewValue!);
            });
        }
    }
}
