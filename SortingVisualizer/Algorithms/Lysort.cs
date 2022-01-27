using System;
using System.Collections.Generic;
using System.Linq;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.Algorithms;

internal class Lysort : IVisualizable
{
    public string Name => "Lysort";

    private void merge(Span<int> run1, Span<int> run2, Span<int> destination)
    {
        int i1 = 0;
        int i2 = 0;
        int j = 0;

        while (i1 < run1.Length && i2 < run2.Length)
        {
            if (run1[i1] <= run2[i2])
            {
                destination[j++] = run1[i1++];
            }
            else
            {
                destination[j++] = run2[i2++];
            }
        }

        if (i1 < run1.Length)
            while (i1 < run1.Length)
            {
                destination[j++] = run1[i1++];
            }

        if (i2 < run2.Length)
            while (i2 < run2.Length)
            {
                destination[j++] = run2[i2++];
            }

        for (int i = 0; i < run1.Length; i++)
            run1[i] = destination[i];
        for (int i = 0; i < run2.Length; i++)
            run2[i] = destination[i + run1.Length];
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
        }

        runs.Add(new RunSlice { StartIndex = startIndex, Length = array.Length - startIndex, IsReversed = direction == -1 });
        foreach (RunSlice run in runs)
        {
            if (run.IsReversed)
            {
                array.AsSpan().Slice(run.StartIndex, run.Length).Reverse();
                yield return VisualizationFrame.From(array);
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
                merge(array.AsSpan().Slice(run1.StartIndex, run1.Length), array.AsSpan().Slice(run2.StartIndex, run2.Length), sorted.AsSpan().Slice(run1.StartIndex, run1.Length + run2.Length));
                runs.RemoveAt(i);
                runs.RemoveAt(i);
                runs.Insert(i, new RunSlice { StartIndex = run1.StartIndex, Length = run1.Length + run2.Length, IsReversed = false });
                yield return VisualizationFrame.From(sorted);
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
