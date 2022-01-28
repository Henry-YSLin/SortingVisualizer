using System;
using System.Collections.Generic;
using System.Linq;
using SortingVisualizer.ViewModels;

namespace SortingVisualizer.ArrayGenerators;

public class ArrayGeneratorManager : ViewModelBase
{
    public List<ArrayGenerator> Generators { get; }

    public ArrayGeneratorManager() : base("Array Generator Repository")
    {
        Generators = typeof(ArrayGenerator).Assembly
            .GetTypes()
            .Where(x => x.IsSubclassOf(typeof(ArrayGenerator)) && !x.IsAbstract)
            .Select(x => (ArrayGenerator?)Activator.CreateInstance(x))
            .Where(x => x != null)
            .Select(x => x!)
            .ToList();
    }
}
