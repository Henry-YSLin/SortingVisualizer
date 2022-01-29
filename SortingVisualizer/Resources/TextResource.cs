using System;
using System.IO;
using System.Windows;
using System.Windows.Resources;

namespace SortingVisualizer.Resources;

public class TextResource
{
    private readonly string location;

    public TextResource(string location)
    {
        this.location = location;
    }

    private string? cache;

    public string? Value
    {
        get
        {
            if (cache != null) return cache;
            Uri uri = new Uri(location, UriKind.Relative);
            StreamResourceInfo? info = Application.GetResourceStream(uri);
            if (info == null) return null;
            StreamReader reader = new StreamReader(info.Stream);
            return cache = reader.ReadToEnd();
        }
    }
}
