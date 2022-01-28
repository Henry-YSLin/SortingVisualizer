namespace SortingVisualizer.Commands;

public interface ICommand : System.Windows.Input.ICommand
{
    string DisplayName { get; }
}

public interface ICommand<in T> : ICommand
{
    bool CanExecute(T? parameter);

    void Execute(T? parameter);
}
