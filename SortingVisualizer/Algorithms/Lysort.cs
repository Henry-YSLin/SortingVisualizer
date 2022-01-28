using System;
using System.Collections.Generic;
using System.Linq;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.Algorithms;

internal class Lysort : IVisualizable
{
    public string Name => "Lysort";

    private static IEnumerable<VisualizationFrame> merge(IList<int> source, IList<int> destination, int start1, int start2, int end)
    {
        int i1 = start1;
        int i2 = start2;
        int j = start1;

        while (i1 < start2 && i2 < end)
        {
            if (source[i1] <= source[i2])
            {
                destination[j++] = source[i1++];
            }
            else
            {
                destination[j++] = source[i2++];
            }

            yield return VisualizationFrame.From(destination, new List<int> { i1, i2 });
        }

        if (i1 < start2)
            while (i1 < start2)
            {
                destination[j++] = source[i1++];
                yield return VisualizationFrame.From(destination);
            }

        if (i2 < end)
            while (i2 < end)
            {
                destination[j++] = source[i2++];
                yield return VisualizationFrame.From(destination);
            }

        for (int i = start1; i < end; i++)
            source[i] = destination[i];
    }

    private static IEnumerable<VisualizationFrame> reverse(IList<int> array, int start, int end)
    {
        for (int i = 0; i < (end - start) / 2; i++)
        {
            (array[start + i], array[end - 1 - i]) = (array[end - 1 - i], array[start + i]);
            yield return VisualizationFrame.From(array, new List<int> { start + i, end - 1 - i });
        }
    }

    public IEnumerable<VisualizationFrame> Run(int[] array, IVisualizer visualizer)
    {
        List<RunSlice> runs = new List<RunSlice>();
        int startIndex = 0;
        int direction = 0; // 1 for ascending, -1 for descending, 0 for undetermined
        for (int i = 1; i < array.Length; i++)
        {
            int thisElement = array[i];
            int lastElement = array[i - 1];
            if (direction == 0)
            {
                if (thisElement > lastElement)
                    direction = 1;
                else if (thisElement < lastElement)
                    direction = -1;
            }
            else if (direction == 1 && thisElement < lastElement || direction == -1 && thisElement > lastElement)
            {
                runs.Add(new RunSlice { StartIndex = startIndex, Length = i - startIndex, IsReversed = direction == -1 });
                startIndex = i;
                direction = 0;
            }

            yield return VisualizationFrame.From(array, new List<int> { i });
        }

        runs.Add(new RunSlice { StartIndex = startIndex, Length = array.Length - startIndex, IsReversed = direction == -1 });
        foreach (RunSlice run in runs)
        {
            if (run.IsReversed)
            {
                foreach (var frame in reverse(array, run.StartIndex, run.StartIndex + run.Length))
                {
                    yield return frame;
                }
            }
        }

        int[] sorted = array.ToArray();
        while (runs.Count > 1)
        {
            for (int i = 0; i < runs.Count; i++)
            {
                if (i == runs.Count - 1) break;
                RunSlice run1 = runs[i];
                RunSlice run2 = runs[i + 1];
                foreach (var frame in merge(array, sorted, run1.StartIndex, run2.StartIndex, run1.StartIndex + run1.Length + run2.Length))
                {
                    yield return frame;
                }

                runs.RemoveAt(i);
                runs.RemoveAt(i);
                runs.Insert(i, new RunSlice { StartIndex = run1.StartIndex, Length = run1.Length + run2.Length, IsReversed = false });
            }
        }
    }

    private struct RunSlice
    {
        public int StartIndex;
        public int Length;
        public bool IsReversed;
    }
}
