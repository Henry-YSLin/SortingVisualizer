using System;
using System.Collections.Generic;
using System.Linq;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.Algorithms;

internal class AlgorithmRepository
{
    public List<IVisualizable> Algorithms { get; }

    private AlgorithmRepository()
    {
        Algorithms = typeof(IVisualizable).Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IVisualizable)) && !x.IsAbstract)
            .Select(x => (IVisualizable?)Activator.CreateInstance(x))
            .Where(x => x != null)
            .Select(x => x!)
            .ToList();
    }

    private static AlgorithmRepository? instance;

    public static AlgorithmRepository Instance => instance ??= new AlgorithmRepository();
}
