using System;
using System.Threading.Tasks;
using GUI.Models;
using GUI.ViewModels;
using GUI.Views;
using Moq;

namespace GUI.Tests;

public class FileTabViewModelTests : GuiTest<App>
{
    [Test]
    public async Task CreationTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var view = new FileTab();
            var file = new FileModel { FilePath = "header.txt" };

            // Act

            var viewModel = new FileTabViewModel(view, file, null, null);

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

            var viewModel = new FileTabViewModel(new FileTab(), null, null, null)
            {
                // Act
                IsSelected = true
            };

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

            var viewModel = new FileTabViewModel(new FileTab(), null, null, null)
            {
                IsSelected = true
            };

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

            var viewModel = new FileTabViewModel(new FileTab(), new FileModel { IsNeedSave = true }, null, null);
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

            var viewModel = new FileTabViewModel(new FileTab(), null, null, null)
            {
                IsSelected = initSelection
            };

            var propertyName = string.Empty;

            viewModel.PropertyChanged += (_, args) => { propertyName = args.PropertyName; };

            // Act

            viewModel.IsSelected = !initSelection;

            // Assert

            Assert.That(propertyName, Is.EqualTo(nameof(viewModel.TabBackground)));
        });
    }

    [Test]
    public async Task HeaderChangedEventTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var viewModel = new FileTabViewModel(new FileTab(), null, null, null);

            var propertyName = string.Empty;

            viewModel.PropertyChanged += (_, args) => { propertyName = args.PropertyName; };

            // Act

            viewModel.NotifyHeaderChanged();

            // Assert

            Assert.That(propertyName, Is.EqualTo(nameof(viewModel.TabHeader)));
        });
    }

    [Test]
    public async Task ForegroundChangedEventTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var viewModel = new FileTabViewModel(new FileTab(), null, null, null);

            var propertyName = string.Empty;

            viewModel.PropertyChanged += (_, args) => { propertyName = args.PropertyName; };

            // Act

            viewModel.NotifyForegroundChanged();

            // Assert

            Assert.That(propertyName, Is.EqualTo(nameof(viewModel.TabForeground)));
        });
    }

    [Test]
    public async Task CommandsTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var selectCommandMock = new Mock<Func<IFileTabViewModel, Task>>();
            var closeCommandMock = new Mock<Func<IFileTabViewModel, Task>>();

            var viewModel = new FileTabViewModel(new FileTab(), null,
                selectCommandMock.Object, closeCommandMock.Object);

            // Act

            viewModel.SelectTabCommand.Execute(null);
            viewModel.CloseTabCommand.Execute(null);

            // Assert

            selectCommandMock.Verify(m => m(viewModel), Times.Once);
            closeCommandMock.Verify(m => m(viewModel), Times.Once);
        });
    }
}