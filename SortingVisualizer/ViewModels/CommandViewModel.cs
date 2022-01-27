using System;
using System.Windows.Input;
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
