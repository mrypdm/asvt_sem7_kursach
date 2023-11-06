using System;
using System.IO;
using System.Threading.Tasks;
using GUI.Models;
using GUI.ViewModels;
using GUI.Views;
using Moq;

namespace GUI.Tests.ViewModels;

public class FileTabViewModelTests
{
    private static FileModel DefaultFile => new()
    {
        FilePath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}main.asm"
    };

    [Test]
    public void CreationTest()
    {
        // Arrange

        var view = new FileTab();
        var file = new FileModel { FilePath = "header.txt" };

        // Act

        var viewModel = new FileTabViewModel(view, file, _ => Task.CompletedTask, _ => Task.CompletedTask);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.File, Is.EqualTo(file));
            Assert.That(viewModel.View, Is.EqualTo(view));
            Assert.That(view.DataContext, Is.EqualTo(viewModel));
            Assert.That(viewModel.TabHeader, Is.EqualTo("header.txt"));
        });
    }

    [Test]
    public void SelectTabBackgroundTest()
    {
        // Arrange

        var viewModel = new FileTabViewModel(new FileTab(), DefaultFile, _ => Task.CompletedTask,
            _ => Task.CompletedTask)
        {
            // Act
            IsSelected = true
        };

        // Arrange

        Assert.That(viewModel.TabBackground, Is.EqualTo(FileTabViewModel.SelectedBackground));
    }

    [Test]
    public void UnselectTabBackgroundTest()
    {
        // Arrange

        var viewModel = new FileTabViewModel(new FileTab(), DefaultFile, _ => Task.CompletedTask,
            _ => Task.CompletedTask)
        {
            IsSelected = true
        };

        // Act

        viewModel.IsSelected = false;

        // Arrange

        Assert.That(viewModel.TabBackground, Is.EqualTo(FileTabViewModel.DefaultBackground));
    }

    [Test]
    public void NeedSaveForegroundTest()
    {
        // Arrange

        var file = DefaultFile;
        file.IsNeedSave = true;

        var viewModel = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);
        Assert.That(viewModel.TabForeground, Is.EqualTo(FileTabViewModel.NeedSaveForeground));

        // Act

        viewModel.File.IsNeedSave = false;
        Assert.That(viewModel.TabForeground, Is.EqualTo(FileTabViewModel.DefaultForeground));
    }

    [Test]
    [TestCase(false)]
    [TestCase(true)]
    public void SelectEventTest(bool initSelection)
    {
        // Arrange

        var viewModel = new FileTabViewModel(new FileTab(), DefaultFile, _ => Task.CompletedTask,
            _ => Task.CompletedTask)
        {
            IsSelected = initSelection
        };

        var propertyAssert = new PropertyChangedAssert(viewModel);

        // Act

        viewModel.IsSelected = !initSelection;

        // Assert

        propertyAssert.Assert(nameof(viewModel.TabBackground));
    }

    [Test]
    public void HeaderChangedEventTest()
    {
        // Arrange

        var viewModel = new FileTabViewModel(new FileTab(), DefaultFile, _ => Task.CompletedTask,
            _ => Task.CompletedTask);
        var propertyAssert = new PropertyChangedAssert(viewModel);

        // Act

        viewModel.NotifyHeaderChanged();

        // Assert

        propertyAssert.Assert(nameof(viewModel.TabHeader));
    }

    [Test]
    public void ForegroundChangedEventTest()
    {
        // Arrange

        var viewModel = new FileTabViewModel(new FileTab(), DefaultFile, _ => Task.CompletedTask,
            _ => Task.CompletedTask);
        var propertyAssert = new PropertyChangedAssert(viewModel);

        // Act

        viewModel.NotifyForegroundChanged();

        // Assert

        propertyAssert.Assert(nameof(viewModel.TabForeground));
    }

    [Test]
    public void CommandsTest()
    {
        // Arrange

        var selectCommand = new Mock<Func<IFileTabViewModel, Task>>();
        var closeCommand = new Mock<Func<IFileTabViewModel, Task>>();

        var viewModel = new FileTabViewModel(new FileTab(), DefaultFile, selectCommand.Object, closeCommand.Object);

        // Act

        viewModel.SelectTabCommand.Execute(null);
        viewModel.CloseTabCommand.Execute(null);

        // Assert

        selectCommand.Verify(m => m(viewModel), Times.Once);
        closeCommand.Verify(m => m(viewModel), Times.Once);
    }
}