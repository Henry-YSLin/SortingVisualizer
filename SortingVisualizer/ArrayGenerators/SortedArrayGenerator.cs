using System.Linq;

namespace SortingVisualizer.ArrayGenerators
{
    internal class SortedArrayGenerator : RandomizedArrayGenerator
    {
        public override string Name => "Sorted Array";
        public override int[] Generate(int length)
        {
            if (random == null) random = new System.Random();
            int[] array = new int[length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(length);
            }
            return array.OrderBy(x => x).ToArray();
        }
    }
}
