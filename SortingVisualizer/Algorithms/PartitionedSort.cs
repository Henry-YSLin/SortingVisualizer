using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SortingVisualizer
{
    internal class PartitionedSort : IVisualizable
    {
        private int[] partition(int[] array, int partitionSize)
        {
            for (int i = 0; i < array.Length; i += partitionSize)
            {
                array.AsSpan().Slice(i, partitionSize).Sort();
            }

            int[] processedArray = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                processedArray[i / partitionSize + i % partitionSize * (array.Length / partitionSize)] = array[i];
            }
            return processedArray;
        }

        public async IAsyncEnumerable<int[]> Run(int[] array, IVisualizer visualizer)
        {
            yield return await visualizer.NewFrame(array);
            await Task.Delay(2000);

            array = partition(array, 20);
            yield return await visualizer.NewFrame(array);
            await Task.Delay(2000);

            for (int i = 1; i < 1000; i++)
            {
                var val = array[i];
                bool done = false;
                for (int j = i - 1; j >= 0 && !done;)
                {
                    if (val < array[j])
                    {
                        array[j + 1] = array[j];
                        j--;
                        array[j + 1] = val;
                        yield return await visualizer.NewFrame(array);
                    }
                    else done = true;
                }
            }
        }
    }
}
