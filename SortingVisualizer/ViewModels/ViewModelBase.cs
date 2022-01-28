using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SortingVisualizer.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    public ViewModelBase(string displayName, bool throwOnInvalidPropertyName = true)
    {
        this.displayName = displayName;
        ThrowOnInvalidPropertyName = throwOnInvalidPropertyName;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string? propertyName)
    {
        verifyPropertyName(propertyName);
        PropertyChangedEventHandler? handler = PropertyChanged;
        if (handler != null)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
    }

    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    private void verifyPropertyName(string? propertyName)
    {
        // Verify that the property name matches a real,
        // public, instance property on this object.
        if (propertyName == null || TypeDescriptor.GetProperties(this)[propertyName] == null)
        {
            string msg = "Invalid property name: " + propertyName;
            if (this.ThrowOnInvalidPropertyName)
                throw new Exception(msg);
            else
                Debug.Fail(msg);
        }
    }

    public void SetAndNotify<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!Equals(field, value))
        {
            field = value;
            OnPropertyChanged(propertyName);
        }
    }

    public bool ThrowOnInvalidPropertyName { get; init; }

    private string displayName;

    public string DisplayName
    {
        get => displayName;
        set => SetAndNotify(ref displayName, value);
    }
}
