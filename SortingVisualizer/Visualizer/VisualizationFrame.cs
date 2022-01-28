using System.Collections.Generic;
using System.Linq;

namespace SortingVisualizer.Visualizer;

public class VisualizationFrame
{
    public VisualizationFrame(int[] array, List<int>? accessedIndices = null)
    {
        Array = array;
        AccessedIndices = accessedIndices ?? new List<int>();
    }

    public int[] Array { get; init; }

    public List<int> AccessedIndices { get; init; }

    public void Deconstruct(out int[] array, out List<int> accessedIndices)
    {
        array = Array;
        accessedIndices = AccessedIndices;
    }

    /**
     * Create a new <see cref="VisualizationFrame"/> from a copy of the provided array.
     */
    public static VisualizationFrame From(IEnumerable<int> array, List<int>? accessedIndices = null)
    {
        return new VisualizationFrame(array.ToArray(), accessedIndices);
    }
}
