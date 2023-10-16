using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUI.Models;
using GUI.ViewModels;

namespace GUI.Views;

/// <summary>
/// File tab view
/// </summary>
public partial class FileTab : UserControl
{
    /// <summary>
    /// Default constructor for designer
    /// </summary>
    public FileTab()
    {
    }
    
    /// <summary>
    /// Creates new instance of file tab
    /// </summary>
    /// <param name="file">File info</param>
    /// <param name="selectCommand">Action on selecting tab</param>
    /// <param name="closeCommand">Action on closing tab</param>
    public FileTab(FileModel file, Func<FileTab, Task> selectCommand, Func<FileTab, Task> closeCommand)
    {
        InitializeComponent();
        ViewModel = new FileTabViewModel(this, file, selectCommand, closeCommand);
        DataContext = ViewModel;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// File info
    /// </summary>
    public FileModel File => ViewModel.File;

    /// <summary>
    /// File tab view model
    /// </summary>
    public FileTabViewModel ViewModel { get; }
}