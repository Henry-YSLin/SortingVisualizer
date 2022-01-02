using System.Threading.Tasks;

namespace SortingVisualizer
{
    internal interface IVisualizer
    {
        public Task<int[]> NewFrame(int[] array);
    }
}
