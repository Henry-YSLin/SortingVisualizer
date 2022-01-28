using System;
using System.Windows.Input;

namespace SortingVisualizer.Commands;

public abstract class CommandBase : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public abstract bool CanExecute(object? parameter);

    public abstract void Execute(object? parameter);

    public abstract string DisplayName { get; }
}

public abstract class CommandBase<T> : CommandBase, ICommand<T>
{
    public override bool CanExecute(object? parameter)
    {
        return CanExecute((T?)parameter);
    }

    public override void Execute(object? parameter)
    {
        Execute((T?)parameter);
    }

    public abstract bool CanExecute(T? parameter);

    public abstract void Execute(T? parameter);
}
