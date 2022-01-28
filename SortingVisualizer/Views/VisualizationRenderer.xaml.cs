using System;
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

    private double clamp(double value, double min, double max)
    {
        return Math.Max(Math.Min(value, max), min);
    }

    private void render(VisualizationFrame frame)
    {
        var group = new GeometryGroup();
        double height = VisualizationPath.ActualHeight - 1;
        int[] array = frame.Array;

        for (int i = 0; i < array.Length; i++)
        {
            Point p0 = new(i, clamp(height - array[i] * height / array.Length, 0, height));
            Point p1 = new(i, height);
            var lineGeometry = new LineGeometry(p0, p1);

            group.Children.Add(lineGeometry);
        }

        VisualizationPath.Data = group;
    }

    public static readonly DependencyProperty FRAME_PROPERTY =
        DependencyProperty.Register("Frame", typeof(VisualizationFrame), typeof(VisualizationRenderer),
            new PropertyMetadata(null, framePropertyChanged));

    public VisualizationFrame? Frame
    {
        get => (VisualizationFrame?)GetValue(FRAME_PROPERTY);
        set => SetValue(FRAME_PROPERTY, value);
    }

    private void framePropertyChanged(VisualizationFrame? frame)
    {
        if (frame != null)
            render(frame);
    }

    private static void framePropertyChanged(
        DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((VisualizationRenderer)d).framePropertyChanged((VisualizationFrame?)e.NewValue);
    }
}
