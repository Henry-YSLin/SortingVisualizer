﻿using System;
using System.Collections.Generic;
using System.Linq;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.Algorithms;

internal class NonRecursiveMergeSort : IVisualizable
{
    public string Name => "Non-recursive Merge Sort";

    private static void merge(Span<int> run1, Span<int> run2, Span<int> destination)
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
        int sliceSize = 1;
        int[] sorted = array.ToArray();
        while (sliceSize < array.Length)
        {
            for (int i = 0; i + sliceSize < array.Length; i += sliceSize * 2)
            {
                merge(array.AsSpan().Slice(i, sliceSize), array.AsSpan().Slice(i + sliceSize, Math.Min(sliceSize, array.Length - i - sliceSize)), sorted.AsSpan().Slice(i, Math.Min(sliceSize * 2, array.Length - i)));
                yield return VisualizationFrame.From(array);
            }
            sliceSize *= 2;
        }
    }
}
