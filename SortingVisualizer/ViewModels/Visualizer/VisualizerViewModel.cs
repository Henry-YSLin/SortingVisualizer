﻿using System;
using System.Threading;
using System.Threading.Tasks;
using SortingVisualizer.Algorithms;
using SortingVisualizer.ArrayGenerators;
using SortingVisualizer.Commands;
using SortingVisualizer.Utilities;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.ViewModels.Visualizer;

public class VisualizerViewModel : ViewModelBase, IVisualizer
{
    [Resolved]
    public AlgorithmManager AlgorithmManager { get; set; } = null!;

    [Resolved]
    public ArrayGeneratorManager ArrayGeneratorManager { get; set; } = null!;

    public VisualizerViewModel() : base("Visualizer")
    {
    }

    public VisualizationSettingsViewModel VisualizationSettings { get; } = new();

    private VisualizationFrame? currentFrame;

    public VisualizationFrame? CurrentFrame
    {
        get => currentFrame;
        set => SetAndNotify(ref currentFrame, value);
    }

    private ICommand? visualizeCommand;

    public ICommand VisualizeCommand => visualizeCommand ??= new RelayCommand(
        "Visualize",
        _ => visualize(),
        _ => VisualizationSettings.GetVisualizationInfo() != null
    );

    private CancellationTokenSource? cancellation;

    private async Task runVisualizationAsync(VisualizationInfo visualization, CancellationToken token)
    {
        int[] array = visualization.ArrayGenerator.Generate(visualization.ArrayLength);
        CurrentFrame = new VisualizationFrame(array);
        token.ThrowIfCancellationRequested();
        foreach (VisualizationFrame frame in visualization.Algorithm.Run(array, this))
        {
            CurrentFrame = frame;
            await Task.Delay(1, token);
        }
    }

    private async void visualize()
    {
        VisualizationInfo? info = VisualizationSettings.GetVisualizationInfo();
        if (info == null) return;
        if (cancellation != null)
        {
            cancellation.Cancel();
            cancellation.Dispose();
        }

        cancellation = new CancellationTokenSource();
        try
        {
            await runVisualizationAsync(info, cancellation.Token);
        }
        catch (OperationCanceledException) { }
    }
}
