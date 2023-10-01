using System;
using Avalonia.Controls;
using GUI.Models;

namespace GUI.Views;

/// <summary>
/// Extended menu item containing a reference to a file
/// </summary>
public class FileTab : MenuItem
{
    protected override Type StyleKeyOverride => typeof(MenuItem);

    /// <summary>
    /// Creates new instance of file tab
    /// </summary>
    /// <param name="file">File info</param>
    public FileTab(FileModel file)
    {
        File = file;
    }

    /// <summary>
    /// File info
    /// </summary>
    public FileModel File { get; }
}