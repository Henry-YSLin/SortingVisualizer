using System;
using System.Diagnostics;
using System.Windows.Input;

namespace SortingVisualizer.Commands;

public abstract class CommandBase<T> : ICommand<T>
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return CanExecute((T?)parameter);
    }

    public void Execute(object? parameter)
    {
        Execute((T?)parameter);
    }

    public abstract bool CanExecute(T? parameter);

    public abstract void Execute(T? parameter);
}
