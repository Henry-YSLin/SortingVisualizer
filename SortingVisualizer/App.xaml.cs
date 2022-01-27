using System.Windows;
using SortingVisualizer.ViewModels;
using SortingVisualizer.Views;

namespace SortingVisualizer;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        MainWindow window = new MainWindow();
        var viewModel = new MainWindowViewModel(delegate { window.Close(); });
        window.DataContext = viewModel;
        window.Show();
    }
}
