using SortingVisualizer.ArrayGenerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SortingVisualizer
{
    internal class ArrayGeneratorRepository
    {
        public List<ArrayGenerator> Generators { get; private set; }

        public ArrayGeneratorRepository()
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            Generators = typeof(ArrayGenerator).Assembly
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(ArrayGenerator)) && !x.IsAbstract)
                .Select(x => (ArrayGenerator?)Activator.CreateInstance(x))
                .Where(x => x != null)
                .ToList();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }
    }
}
