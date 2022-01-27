using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SortingVisualizer.Algorithms;
using SortingVisualizer.ArrayGenerators;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.Views;

public partial class Visualizer : UserControl, IVisualizer
{
    private readonly AlgorithmRepository algorithmRepository = AlgorithmRepository.Instance;
    private readonly ArrayGeneratorRepository arrayGeneratorRepository = ArrayGeneratorRepository.Instance;

    public List<IVisualizable> Algorithms => algorithmRepository.Algorithms;
    public List<ArrayGenerator> Generators => arrayGeneratorRepository.Generators;
    public bool IsPaused { get; set; }
    public IVisualizable? Algorithm { get; set; }
    public ArrayGenerator? Generator { get; set; }
    public int ArrayLength { get; set; }

    public Visualizer()
    {
        InitializeComponent();
    }

    private CancellationTokenSource? cancellation;

    private async Task runVisualization(VisualizationInfo visualization, CancellationToken token)
    {
        int[] array = visualization.ArrayGenerator.Generate(ArrayLength);
        Renderer.Render(new VisualizationFrame(array));
        token.ThrowIfCancellationRequested();
        foreach (VisualizationFrame frame in visualization.Algorithm.Run(array, this))
        {
            Renderer.Render(frame);
            await Task.Delay(1, token);
        }
    }

    private async void ButtonVisualize_Click(object sender, RoutedEventArgs e)
    {
        if (Algorithm == null) return;
        if (Generator == null) return;
        if (cancellation != null)
        {
            cancellation.Cancel();
            cancellation.Dispose();
        }

        cancellation = new CancellationTokenSource();
        try
        {
            await runVisualization(new VisualizationInfo(Algorithm, Generator, ArrayLength), cancellation.Token);
        }
        catch (OperationCanceledException) { }
    }
}
