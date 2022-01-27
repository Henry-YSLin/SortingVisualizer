using System.Collections.Generic;
using JetBrains.Annotations;

namespace SortingVisualizer.Visualizer;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public interface IVisualizable
{
    public string Name { get; }
    public IEnumerable<VisualizationFrame> Run(int[] array, IVisualizer visualizer);
}
