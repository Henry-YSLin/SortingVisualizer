using ICSharpCode.AvalonEdit.Document;
using SortingVisualizer.Commands;
using SortingVisualizer.Editor;

namespace SortingVisualizer.ViewModels.Editor;

public class EditorViewModel : ViewModelBase
{
    public EditorViewModel() : base("Editor")
    {
    }

    private TextDocument code = new(@"#include <iostream>

int main() {
	std::cout << ""hi"";
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
    }
}
