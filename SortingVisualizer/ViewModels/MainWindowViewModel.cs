using System;

namespace SortingVisualizer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(Action requestClose) : base("Main Window")
    {
        RequestClose = requestClose;
    }

    public Action RequestClose { get; init; }
}
