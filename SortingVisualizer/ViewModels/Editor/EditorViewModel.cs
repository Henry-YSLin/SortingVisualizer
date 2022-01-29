using System;
using System.Linq;
using ICSharpCode.AvalonEdit.Document;
using SortingVisualizer.Algorithms;
using SortingVisualizer.Commands;
using SortingVisualizer.Editor;
using SortingVisualizer.Resources;
using SortingVisualizer.Utilities;

namespace SortingVisualizer.ViewModels.Editor;

public class EditorViewModel : ViewModelBase
{
    [Resolved]
    public AlgorithmManager AlgorithmManager { get; set; } = null!;

    public EditorViewModel() : base("Editor")
    {
    }

    private TextDocument code = new(ResourceStore.ALGORITHM_CPP.Value);

    public TextDocument Code
    {
        get => code;
        set => SetAndNotify(ref code, value);
    }

    private ICommand? loadCommand;

    public ICommand LoadCommand => loadCommand ??= new RelayCommand(
        "Compile & Load",
        _ => load(),
        _ => true
    );

    private async void load()
    {
        var existingAlgorithm = AlgorithmManager.Algorithms.FirstOrDefault(x => x is NativeLibraryAlgorithm);
        if (existingAlgorithm != null)
        {
            AlgorithmManager.Remove(existingAlgorithm);
        }

        await new CppCodeCompiler(code.Text).CompileAsync();
        var nativeAlgorithm = new NativeLibraryAlgorithm("algorithm.dll");
        AlgorithmManager.Add(nativeAlgorithm);
    }
}
