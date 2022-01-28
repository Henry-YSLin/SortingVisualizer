using System.Reflection;
using System.Windows;
using Autofac;
using Autofac.Core;
using SortingVisualizer.Algorithms;
using SortingVisualizer.ArrayGenerators;
using SortingVisualizer.Utilities;
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
        var builder = new ContainerBuilder();

        builder.RegisterInstance(new AlgorithmManager())
            .AsSelf()
            .PropertiesAutowired(
                (propInfo, _) => propInfo.GetCustomAttribute(typeof(ResolvedAttribute)) != null);
        builder.RegisterInstance(new ArrayGeneratorManager())
            .AsSelf()
            .PropertiesAutowired(
                (propInfo, _) => propInfo.GetCustomAttribute(typeof(ResolvedAttribute)) != null);
        builder.RegisterAssemblyTypes(typeof(App).Assembly)
            .Where(x => x.Name.EndsWith("ViewModel"))
            .AsSelf()
            .PropertiesAutowired(
                (propInfo, _) => propInfo.GetCustomAttribute(typeof(ResolvedAttribute)) != null);

        var container = builder.Build();

        var scope = container.BeginLifetimeScope();

        MainWindow window = new MainWindow();
        var viewModel = scope.Resolve<MainWindowViewModel>(new NamedParameter("requestClose", () =>
        {
            window.Close();
            scope.Dispose();
        }));
        window.DataContext = viewModel;
        window.Show();
    }
}
