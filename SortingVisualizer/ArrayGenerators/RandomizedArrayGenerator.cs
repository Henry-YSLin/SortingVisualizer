using System;

namespace SortingVisualizer.ArrayGenerators
{
    public abstract class RandomizedArrayGenerator : ArrayGenerator
    {
        protected Random? random;
        public void SetRandom(Random random)
        {
            this.random = random;
        }
    }
}
