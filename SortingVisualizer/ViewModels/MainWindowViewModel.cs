using System;
using SortingVisualizer.Utilities;
using SortingVisualizer.ViewModels.Editor;
using SortingVisualizer.ViewModels.Visualizer;

namespace SortingVisualizer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    [Resolved]
    public VisualizerViewModel Visualizer { get; set; } = null!;

    [Resolved]
    public EditorViewModel Editor { get; set; } = null!;

    public MainWindowViewModel(Action requestClose) : base("Main Window")
    {
        RequestClose = requestClose;
    }

    public Action RequestClose { get; init; }
}
