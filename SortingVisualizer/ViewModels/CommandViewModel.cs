using System;
using System.Windows.Input;
using JetBrains.Annotations;
using SortingVisualizer.Commands;

namespace SortingVisualizer.ViewModels;

public class CommandViewModel<T> : ViewModelBase
{
    public CommandViewModel(string displayName, ICommand<T> command) : base(displayName)
    {
        Command = command ?? throw new ArgumentNullException(nameof(command));
    }

    public ICommand<T> Command { get; private set; }
}

public class CommandViewModel : CommandViewModel<object>
{
    public CommandViewModel(string displayName, ICommand<object> command) : base(displayName, command)
    {
    }
}
