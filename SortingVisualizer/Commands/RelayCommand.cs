using System;

namespace SortingVisualizer.Commands;

public class RelayCommand<T> : CommandBase<T>
{
    private readonly Action<T?> execute;
    private readonly Predicate<T?>? canExecute;

    public RelayCommand(string displayName, Action<T?> execute, Predicate<T?>? canExecute = null)
    {
        DisplayName = displayName;
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

    public override string DisplayName { get; }
}

public class RelayCommand : RelayCommand<object>
{
    public RelayCommand(string displayName, Action<object?> execute, Predicate<object?>? canExecute = null) : base(displayName, execute, canExecute)
    {
    }
}
