using SortingVisualizer.ArrayGenerators;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.ViewModels;

public class VisualizationSettingsViewModel : ViewModelBase
{
    private IVisualizable? algorithm;

    public IVisualizable? Algorithm
    {
        get => algorithm;
        set => SetAndNotify(ref algorithm, value);
    }

    private ArrayGenerator? generator;

    public ArrayGenerator? Generator
    {
        get => generator;
        set => SetAndNotify(ref generator, value);
    }

    private int arrayLength;

    public int ArrayLength
    {
        get => arrayLength;
        set => SetAndNotify(ref arrayLength, value);
    }

    public VisualizationSettingsViewModel() : base("Visualization Settings")
    {
    }

    public VisualizationInfo? GetVisualizationInfo()
    {
        if (algorithm == null) return null;
        if (generator == null) return null;
        return new VisualizationInfo(algorithm, generator, arrayLength);
    }
}
