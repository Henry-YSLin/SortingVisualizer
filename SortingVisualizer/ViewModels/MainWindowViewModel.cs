using System;
using SortingVisualizer.ViewModels.Visualizer;

namespace SortingVisualizer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private VisualizerViewModel visualizer;

    public VisualizerViewModel Visualizer
    {
        get => visualizer;
        set => SetAndNotify(ref visualizer, value);
    }

    public MainWindowViewModel(VisualizerViewModel visualizer, Action requestClose) : base("Main Window")
    {
        RequestClose = requestClose;
        this.visualizer = visualizer;
    }

    public Action RequestClose { get; init; }
}
