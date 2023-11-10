using System;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GUI.Models;
using GUI.ViewModels;
using GUI.Views;
using Moq;

namespace GUI.Tests.ViewModels;

public class FileTabViewModelTests : GuiTest<App>
{
    private static FileModel DefaultFile => new()
    {
        FilePath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}main.asm"
    };

    [Test]
    public async Task CreationTest()
    {
        await RunTest(() =>
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
        });
    }

    [Test]
    public async Task SelectTabBackgroundTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var viewModel = CreateViewModel();

            // Act

            viewModel.IsSelected = true;

            // Arrange

            Assert.That(viewModel.TabBackground, Is.EqualTo(FileTabViewModel.SelectedBackground));
        });
    }

    [Test]
    public async Task UnselectTabBackgroundTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var viewModel = CreateViewModel();
            viewModel.IsSelected = true;

            // Act

            viewModel.IsSelected = false;

            // Arrange

            Assert.That(viewModel.TabBackground, Is.EqualTo(FileTabViewModel.DefaultBackground));
        });
    }

    [Test]
    public async Task NeedSaveForegroundTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var file = DefaultFile;
            file.IsNeedSave = true;

            var viewModel = CreateViewModel(file);
            Assert.That(viewModel.TabForeground, Is.EqualTo(FileTabViewModel.NeedSaveForeground));

            // Act

            viewModel.File.IsNeedSave = false;
            Assert.That(viewModel.TabForeground, Is.EqualTo(FileTabViewModel.DefaultForeground));
        });
    }

    [Test]
    [TestCase(false)]
    [TestCase(true)]
    public async Task SelectEventTest(bool initSelection)
    {
        await RunTest(() =>
        {
            // Arrange

            var viewModel = CreateViewModel();
            viewModel.IsSelected = initSelection;

            var propertyAssert = new PropertyChangedAssert(viewModel);

            // Act

            viewModel.IsSelected = !initSelection;

            // Assert

            propertyAssert.Assert(nameof(viewModel.TabBackground));
        });
    }

    [Test]
    public async Task HeaderChangedEventTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var viewModel = CreateViewModel();
            var propertyAssert = new PropertyChangedAssert(viewModel);

            // Act

            viewModel.NotifyHeaderChanged();

            // Assert

            propertyAssert.Assert(nameof(viewModel.TabHeader));
        });
    }

    [Test]
    public async Task ForegroundChangedEventTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var viewModel = CreateViewModel();
            var propertyAssert = new PropertyChangedAssert(viewModel);

            // Act

            viewModel.NotifyForegroundChanged();

            // Assert

            propertyAssert.Assert(nameof(viewModel.TabForeground));
        });
    }

    [Test]
    public async Task CommandsTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var selectCommand = new Mock<Func<IFileTabViewModel, Task>>();
            var closeCommand = new Mock<Func<IFileTabViewModel, Task>>();

            var viewModel = CreateViewModel(null, selectCommand.Object, closeCommand.Object);

            // Act

            await viewModel.SelectTabCommand.Execute();
            await viewModel.CloseTabCommand.Execute();

            // Assert

            selectCommand.Verify(m => m(viewModel), Times.Once);
            closeCommand.Verify(m => m(viewModel), Times.Once);
        });
    }

    private static FileTabViewModel CreateViewModel(FileModel file = null,
        Func<FileTabViewModel, Task> selectCommand = null,
        Func<FileTabViewModel, Task> closeCommand = null) =>
        new(new FileTab(),
            file ?? DefaultFile,
            selectCommand ?? (_ => Task.CompletedTask),
            closeCommand ?? (_ => Task.CompletedTask));
}