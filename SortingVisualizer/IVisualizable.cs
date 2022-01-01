using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SortingVisualizer
{
    internal interface IVisualizable
    {
        public IAsyncEnumerable<int[]> Run(int[] array, Func<int[], Task<int[]>> snapshot);
    }
}
