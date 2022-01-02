using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SortingVisualizer.Algorithms
{
    internal class QuickSort : IVisualizable
    {
        public string Name => "Quick Sort";
        public async IAsyncEnumerable<int[]> Run(int[] array, IVisualizer visualizer, [EnumeratorCancellation] CancellationToken token)
        {
            foreach (int[] snapshot in quickSort(array, 0, array.Length - 1, visualizer))
            {
                token.ThrowIfCancellationRequested();
                yield return await visualizer.NewFrame(array);
            }
            yield return await visualizer.NewFrame(array);
        }

        private IEnumerable<int[]> quickSort(int[] array, int left, int right, IVisualizer visualizer)
        {
            if (left < right)
            {
                int pivot = partition(array, left, right);

                yield return array.ToArray();

                if (pivot > left)
                    foreach (int[] snapshot in quickSort(array, left, pivot, visualizer))
                    {
                        yield return snapshot;
                    }

                if (pivot < right)
                    foreach (int[] snapshot in quickSort(array, pivot + 1, right, visualizer))
                    {
                        yield return snapshot;
                    }
            }
        }

        private int partition(int[] array, int left, int right)
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
                    int temp = array[left];
                    array[left] = array[right];
                    array[right] = temp;
                }
                else return right;
            }
        }
    }
}
