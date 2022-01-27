using System;
using System.Collections.Generic;
using System.Linq;

namespace SortingVisualizer.ArrayGenerators;

internal class ArrayGeneratorRepository
{
    public List<ArrayGenerator> Generators { get; }

    private ArrayGeneratorRepository()
    {
        Generators = typeof(ArrayGenerator).Assembly
            .GetTypes()
            .Where(x => x.IsSubclassOf(typeof(ArrayGenerator)) && !x.IsAbstract)
            .Select(x => (ArrayGenerator?)Activator.CreateInstance(x))
            .Where(x => x != null)
            .Select(x => x!)
            .ToList();
    }

    private static ArrayGeneratorRepository? instance;

    public static ArrayGeneratorRepository Instance => instance ??= new ArrayGeneratorRepository();
}
