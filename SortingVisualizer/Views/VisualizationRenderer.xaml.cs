using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.Views;

public partial class VisualizationRenderer : UserControl
{
    public VisualizationRenderer()
    {
        InitializeComponent();
    }

    public void Render(VisualizationFrame frame)
    {
        var group = new GeometryGroup();
        double height = VisualizationPath.ActualHeight - 1;
        int[] array = frame.Array;

        for (int i = 0; i < array.Length; i++)
        {
            Point p0 = new(i, height - array[i] * height / array.Length);
            Point p1 = new(i, height);
            var lineGeometry = new LineGeometry(p0, p1);

            group.Children.Add(lineGeometry);
        }

        VisualizationPath.Data = group;
    }
}
