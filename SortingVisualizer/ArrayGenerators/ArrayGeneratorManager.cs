using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SortingVisualizer.ViewModels;

namespace SortingVisualizer.ArrayGenerators;

public class ArrayGeneratorManager : ViewModelBase
{
    public ObservableCollection<ArrayGenerator> Generators { get; }

    public ArrayGeneratorManager() : base("Array Generator Repository")
    {
        Generators = new ObservableCollection<ArrayGenerator>(typeof(ArrayGenerator).Assembly
            .GetTypes()
            .Where(x => x.IsSubclassOf(typeof(ArrayGenerator)) && !x.IsAbstract)
            .Select(x => (ArrayGenerator?)Activator.CreateInstance(x))
            .Where(x => x != null)
            .Select(x => x!));
    }
}
