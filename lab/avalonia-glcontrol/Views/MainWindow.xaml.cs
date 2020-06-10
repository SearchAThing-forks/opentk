using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

// created with `dotnet new avalonia.mvvm -n example-avalonia-opengl`
// refs: https://github.com/AvaloniaUI/avalonia-dotnet-templates

namespace example_avalonia_opengl.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void click1(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine($"{DateTime.Now}: click");            

            var ctl = this.FindControl<GLControl>("glctl");
            
            ctl.win.MakeCurrent();
            
            ctl.InvalidateVisual();            
        }
    }
}