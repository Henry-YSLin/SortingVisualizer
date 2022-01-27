using System.Windows.Input;

namespace SortingVisualizer.Commands;

public interface ICommand<in T> : ICommand
{
    bool CanExecute(T? parameter);

    void Execute(T? parameter);
}
