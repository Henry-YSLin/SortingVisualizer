using System.Collections.Generic;
using System.Linq;

namespace SortingVisualizer.Visualizer;

public class VisualizationFrame
{
    public VisualizationFrame(int[] array)
    {
        Array = array;
    }

    public int[] Array { get; init; }

    public void Deconstruct(out int[] array)
    {
        array = Array;
    }

    /**
     * Create a new <see cref="VisualizationFrame"/> from a copy of the provided array.
     */
    public static VisualizationFrame From(IEnumerable<int> array)
    {
        return new VisualizationFrame(array.ToArray());
    }
}
