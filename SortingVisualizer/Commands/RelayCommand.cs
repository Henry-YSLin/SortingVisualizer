using System;

namespace SortingVisualizer.Commands;

public class RelayCommand<T> : CommandBase<T>
{
    private readonly Action<T?> execute;
    private readonly Predicate<T?>? canExecute;

    public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute = null)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute;
    }

    public override bool CanExecute(T? parameter)
    {
        return canExecute?.Invoke(parameter) ?? true;
    }

    public override void Execute(T? parameter)
    {
        execute(parameter);
    }
}

public class RelayCommand : RelayCommand<object>
{
    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null) : base(execute, canExecute)
    {
    }
}
