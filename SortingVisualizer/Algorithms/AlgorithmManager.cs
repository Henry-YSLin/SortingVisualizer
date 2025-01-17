﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using SortingVisualizer.ViewModels;
using SortingVisualizer.Visualizer;

namespace SortingVisualizer.Algorithms;

public class AlgorithmManager : ViewModelBase
{
    public ObservableCollection<IVisualizable> Algorithms { get; }

    public AlgorithmManager() : base("Algorithm Repository")
    {
        Algorithms = new ObservableCollection<IVisualizable>(typeof(IVisualizable).Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IVisualizable)) && !x.IsAbstract && x.GetConstructor(Array.Empty<Type>()) != null)
            .Select(x => (IVisualizable?)Activator.CreateInstance(x))
            .Where(x => x != null)
            .Select(x => x!));
    }

    public void Remove(IVisualizable algorithm)
    {
        Algorithms.Remove(algorithm);
        if (algorithm is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    public void Add(IVisualizable algorithm)
    {
        Algorithms.Add(algorithm);
    }
}
