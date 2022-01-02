using System;
using System.Collections.Generic;
using System.Linq;

namespace SortingVisualizer
{
    internal class AlgorithmRepository
    {
        public List<IVisualizable> Algorithms { get; private set; }

        public AlgorithmRepository()
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            Algorithms = typeof(IVisualizable).Assembly
                .GetTypes()
                .Where(x => x.IsAssignableTo(typeof(IVisualizable)) && !x.IsAbstract)
                .Select(x => (IVisualizable?)Activator.CreateInstance(x))
                .Where(x => x != null)
                .ToList();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }
    }
}
