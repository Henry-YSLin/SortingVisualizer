using System.Threading.Tasks;

namespace SortingVisualizer
{
    public interface IVisualizer
    {
        public Task<int[]> NewFrame(int[] array);
    }
}
