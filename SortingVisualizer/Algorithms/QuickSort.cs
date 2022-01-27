using System.Collections.Generic;
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

        int pivot = partition(array, left, right);

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

    private static int partition(IList<int> array, int left, int right)
    {
        int pivot = array[left];
        while (true)
        {
            while (array[left] < pivot)
                left++;

            while (array[right] > pivot)
                right--;

            if (left < right)
            {
                if (array[left] == array[right])
                {
                    left++;
                    right--;
                }

                (array[left], array[right]) = (array[right], array[left]);
            }
            else return right;
        }
    }
}
