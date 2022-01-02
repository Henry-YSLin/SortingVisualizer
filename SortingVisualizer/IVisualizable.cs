using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SortingVisualizer
{
    public interface IVisualizable
    {
        public string Name { get; }
        public IAsyncEnumerable<int[]> Run(int[] array, IVisualizer visualizer, CancellationToken token);
    }
}
