using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace SD.Controls.Themes
{
    public class ControlThemes : Styles
    {
        public ControlThemes(IServiceProvider? sp = null)
        {
            AvaloniaXamlLoader.Load(sp, this);
        }
    }
}
