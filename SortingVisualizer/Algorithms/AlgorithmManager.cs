using System;
using System.Collections.Generic;
using System.Linq;
using SortingVisualizer.ViewModels;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.Algorithms;

public class AlgorithmManager : ViewModelBase
{
    public List<IVisualizable> Algorithms { get; }

    public AlgorithmManager() : base("Algorithm Repository")
    {
        Algorithms = typeof(IVisualizable).Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IVisualizable)) && !x.IsAbstract)
            .Select(x => (IVisualizable?)Activator.CreateInstance(x))
            .Where(x => x != null)
            .Select(x => x!)
            .ToList();
    }
}
