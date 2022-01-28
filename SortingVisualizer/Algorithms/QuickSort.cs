using System.Collections.Generic;
using System.Windows.Documents;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.Algorithms;

internal class QuickSort : IVisualizable
{
    public string Name => "Quick Sort";

    public IEnumerable<VisualizationFrame> Run(int[] array, IVisualizer visualizer)
    {
        foreach (VisualizationFrame frame in sort(array, 0, array.Length - 1))
        {
            yield return frame;
        }

        yield return VisualizationFrame.From(array);
    }

    private static IEnumerable<VisualizationFrame> sort(IList<int> array, int left, int right)
    {
        if (left >= right)
            yield break;

        int pivot;
        foreach (var frame in partition(array, left, right, out pivot))
        {
            yield return frame;
        }

        yield return VisualizationFrame.From(array);

        if (pivot > left)
            foreach (VisualizationFrame frame in sort(array, left, pivot))
            {
                yield return frame;
            }

        if (pivot < right)
            foreach (VisualizationFrame frame in sort(array, pivot + 1, right))
            {
                yield return frame;
            }
    }

    private static IEnumerable<VisualizationFrame> partition(IList<int> array, int left, int right, out int mid)
    {
        var frames = new List<VisualizationFrame>();
        int pivot = array[left];
        while (true)
        {
            List<int> accessed = new();
            while (array[left] < pivot)
            {
                accessed.Add(left);
                left++;
            }

            while (array[right] > pivot)
            {
                accessed.Add(right);
                right--;
            }

            if (left < right)
            {
                if (array[left] == array[right])
                {
                    accessed.Add(left);
                    accessed.Add(right);
                    left++;
                    right--;
                }

                (array[left], array[right]) = (array[right], array[left]);
                frames.Add(VisualizationFrame.From(array, accessed));
            }
            else
            {
                mid = right;
                return frames;
            }
        }
    }
}
