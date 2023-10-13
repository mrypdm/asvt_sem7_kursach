using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUI.Models;
using GUI.ViewModels;

namespace GUI.Views;

public partial class FileTab : UserControl
{
    private readonly FileTabViewModel _viewModel;
    
    public FileTab(FileModel file, Func<FileTab, Task> selectCommand, Func<FileTab, Task> closeCommand)
    {
        InitializeComponent();
        _viewModel = new FileTabViewModel(this, file, selectCommand, closeCommand);
        DataContext = _viewModel;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public FileModel File => _viewModel.File;
}