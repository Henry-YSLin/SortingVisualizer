using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingVisualizer.ArrayGenerators
{
    public abstract class ArrayGenerator
    {
        public abstract string Name { get; }
        public abstract int[] Generate(int length);
    }
}
