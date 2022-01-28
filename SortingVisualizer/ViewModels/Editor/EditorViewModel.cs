using System;
using System.Runtime.InteropServices;
using ICSharpCode.AvalonEdit.Document;
using SortingVisualizer.Algorithms;
using SortingVisualizer.Commands;
using SortingVisualizer.Editor;
using SortingVisualizer.Utilities;

namespace SortingVisualizer.ViewModels.Editor;

public class EditorViewModel : ViewModelBase
{
    public EditorViewModel() : base("Editor")
    {
    }

    private TextDocument code = new(@"#define EXPORT __declspec(dllexport)

#include <vector>

using namespace std;

int main() {}

int *newFrame(int arr[], int size)
{
    int *newArr = new int[size];
    for (int i = 0; i < size; i++)
    {
        newArr[i] = arr[i];
    }
    return newArr;
}

vector<int *> runAlgorithm(int arr[], int size)
{
    vector<int *> frames;
    for (int i = 0; i < size; i++)
    {
        arr[i] = 0;
        frames.push_back(newFrame(arr, size));
    }
    return frames;
}

extern ""C""
    {
        struct EXPORT CppVisualization
        {
        int Length;
        int **Arrays;
    };

    EXPORT CppVisualization run(int arr[], int size)
    {
        vector<int *> ret = runAlgorithm(arr, size);

        CppVisualization vis;
        vis.Length = ret.size();
        vis.Arrays = new int *[ret.size()];
        for (int i = 0; i < ret.size(); i++)
        {
            vis.Arrays[i] = ret[i];
        }

        vector<int *>().swap(ret);

        return vis;
    }
}");

    public TextDocument Code
    {
        get => code;
        set => SetAndNotify(ref code, value);
    }

    private ICommand? compileCommand;

    public ICommand CompileCommand => compileCommand ??= new RelayCommand(
        "Compile",
        _ => compile(),
        _ => Code.TextLength > 0
    );

    private async void compile()
    {
        await new CppCodeCompiler(code.Text).CompileAsync();
    }

    private ICommand? loadCommand;

    public ICommand LoadCommand => loadCommand ??= new RelayCommand(
        "Load",
        _ => load(),
        _ => true
    );

    private void load()
    {
        using var cppExternalAlgorithm = new CppExternalAlgorithm("algorithm.dll");
        int[] array = { 1, 2, 4, 12, 67, 3, 7 };
        cppExternalAlgorithm.Run(array, null!);
    }
}
