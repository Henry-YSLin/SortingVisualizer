using System.Linq;

namespace SortingVisualizer.ArrayGenerators
{
    internal class AlmostSortedArrayGenerator : SortedArrayGenerator
    {
        public override string Name => "Almost Sorted Array";
        public override int[] Generate(int length)
        {
            if (random == null) random = new System.Random();
            return base.Generate(length).Select(x => x + random.Next(50) - 25).ToArray();
        }
    }
}
