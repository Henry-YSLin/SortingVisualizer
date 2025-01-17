﻿using System;
using System.Collections.Generic;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.Algorithms;

internal class PartitionedSort : IVisualizable
{
    public string Name => "Partitioned Sort";

    private static int[] partitionSort(int[] array, int partitionSize)
    {
        for (int i = 0; i < array.Length; i += partitionSize)
        {
            array.AsSpan().Slice(i, partitionSize).Sort();
        }
        return array;
    }

    private static int[] partitionDistribute(int[] array, int partitionSize)
    {
        int[] processedArray = new int[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            processedArray[i / partitionSize + i % partitionSize * (array.Length / partitionSize)] = array[i];
        }
        return processedArray;
    }

    public IEnumerable<VisualizationFrame> Run(int[] array, IVisualizer visualizer)
    {
        array = partitionSort(array, 20);
        yield return VisualizationFrame.From(array);
        array = partitionDistribute(array, 20);
        yield return VisualizationFrame.From(array);
        array = partitionSort(array, 20);
        yield return VisualizationFrame.From(array);
        array = partitionDistribute(array, 20);
        yield return VisualizationFrame.From(array);
        array = partitionDistribute(array, 50);
        yield return VisualizationFrame.From(array);

        for (int i = 1; i < array.Length; i++)
        {
            int val = array[i];
            bool done = false;
            for (int j = i - 1; j >= 0 && !done;)
            {
                if (val < array[j])
                {
                    array[j + 1] = array[j];
                    j--;
                    array[j + 1] = val;
                    yield return VisualizationFrame.From(array);
                }
                else done = true;
            }
        }
    }
}
