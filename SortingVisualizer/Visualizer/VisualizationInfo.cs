using System;
using SortingVisualizer.ArrayGenerators;

namespace SortingVisualizer.Visualizer;

public class VisualizationInfo
{
    public IVisualizable Algorithm { get; set; }
    public ArrayGenerator ArrayGenerator { get; set; }
    public int ArrayLength { get; set; }

    public VisualizationInfo(IVisualizable algorithm, ArrayGenerator arrayGenerator, int arrayLength)
    {
        Algorithm = algorithm;
        ArrayGenerator = arrayGenerator;
        ArrayLength = arrayLength;
    }
}
