using System;

namespace SortingVisualizer.ArrayGenerators;

public abstract class RandomizedArrayGenerator : ArrayGenerator
{
    protected Random? Random;
    public void SetRandom(Random random)
    {
        this.Random = random;
    }
}