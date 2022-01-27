using System.Linq;

namespace SortingVisualizer.ArrayGenerators;

internal class AlmostSortedArrayGenerator : SortedArrayGenerator
{
    public override string Name => "Almost Sorted Array";
    public override int[] Generate(int length)
    {
        if (Random == null) Random = new System.Random();
        return base.Generate(length).Select(x => x + Random.Next(50) - 25).ToArray();
    }
}