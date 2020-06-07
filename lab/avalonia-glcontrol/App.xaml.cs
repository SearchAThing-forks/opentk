using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using example_avalonia_opengl.ViewModels;
using example_avalonia_opengl.Views;

// created with `dotnet new avalonia.mvvm -n example-avalonia-opengl`
// refs: https://github.com/AvaloniaUI/avalonia-dotnet-templates

namespace example_avalonia_opengl
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}