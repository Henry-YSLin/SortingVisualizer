using System.Linq;

namespace SortingVisualizer.ArrayGenerators;

internal class ReverselySortedArrayGenerator : RandomizedArrayGenerator
{
    public override string Name => "Reversely Sorted Array";
    public override int[] Generate(int length)
    {
        Random ??= new System.Random();
        var array = new int[length];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = Random.Next(length);
        }
        return array.OrderByDescending(x => x).ToArray();
    }
}