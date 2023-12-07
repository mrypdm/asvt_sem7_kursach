using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Devices.Providers;
using Devices.Validators;
using Domain.Models;
using Domain.Providers;
using GUI.Exceptions;
using GUI.Managers;
using GUI.MessageBoxes;
using GUI.Models;
using GUI.Providers;
using GUI.ViewModels;
using GUI.Views;
using Moq;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using ReactiveUI;
using Shared.Helpers;

namespace GUI.Tests.ViewModels;

public class MainWindowViewModelTests : GuiTest<App>
{
    private static FileModel DefaultFile => new()
    {
        FilePath = "test.asm",
        Text = "Test"
    };

    [SetUp]
    public void SetUp()
    {
        SettingsManager.Create(new EditorOptions(), new CommandLineOptions());
    }

    [Test]
    public async Task CreateFileTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var file = DefaultFile;

            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(m => m.CreateFile(It.IsAny<IStorageProvider>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(file);

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>());

            var projectManager = new Mock<IProjectManager>();

            var viewModel = CreateViewModel(tabManager.Object, projectManager.Object, fileManager.Object);

            // Act

            await viewModel.CreateFileCommand.Execute();

            // Assert

            fileManager.Verify(
                m => m.CreateFile(viewModel.View.StorageProvider, It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
            tabManager.Verify(
                m => m.CreateTab(file, It.IsAny<Func<IFileTabViewModel, Task>>(),
                    It.IsAny<Func<IFileTabViewModel, Task>>()),
                Times.Once);
            projectManager.Verify(m => m.AddFileToProject(file.FilePath), Times.Once);
            projectManager.Verify(m => m.SaveProjectAsync(), Times.Once);
        });
    }

    [Test]
    public async Task CreateFileAbortTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(m => m.CreateFile(It.IsAny<IStorageProvider>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(null as FileModel);

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>());

            var projectManager = new Mock<IProjectManager>();

            var viewModel = CreateViewModel(tabManager.Object, projectManager.Object, fileManager.Object);

            // Act

            await viewModel.CreateFileCommand.Execute();

            // Assert

            fileManager.Verify(
                m => m.CreateFile(viewModel.View.StorageProvider, It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
            tabManager.Verify(
                m => m.CreateTab(It.IsAny<FileModel>(), It.IsAny<Func<IFileTabViewModel, Task>>(),
                    It.IsAny<Func<IFileTabViewModel, Task>>()),
                Times.Never);
            projectManager.Verify(m => m.AddFileToProject(It.IsAny<string>()), Times.Never);
            projectManager.Verify(m => m.SaveProjectAsync(), Times.Never);
        });
    }

    [Test]
    public async Task OpenFileTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var file = DefaultFile;

            var fileManager = new Mock<IFileManager>();
            fileManager.Setup(m => m.OpenFilesAsync(It.IsAny<IStorageProvider>())).ReturnsAsync(new[] { file });

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>());

            var viewModel = CreateViewModel(tabManager.Object, null, fileManager.Object);

            // Act

            await viewModel.OpenFileCommand.Execute();

            // Assert

            fileManager.Verify(m => m.OpenFilesAsync(viewModel.View.StorageProvider), Times.Once);
            tabManager.Verify(
                m => m.CreateTab(file, It.IsAny<Func<IFileTabViewModel, Task>>(),
                    It.IsAny<Func<IFileTabViewModel, Task>>()),
                Times.Once);
        });
    }

    [Test]
    [TestCase("Reopen", true)]
    [TestCase("Skip", false)]
    public async Task OpenFileAlreadyExistsTest(string messageBoxResult, bool shouldUpdate)
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var file = DefaultFile;
            var tab = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var newFile = DefaultFile;
            newFile.Text = "NewText";

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(m => m.OpenFilesAsync(It.IsAny<IStorageProvider>()))
                .ReturnsAsync(new[] { newFile });

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>());
            tabManager
                .Setup(m => m.CreateTab(It.IsAny<FileModel>(), It.IsAny<Func<IFileTabViewModel, Task>>(),
                    It.IsAny<Func<IFileTabViewModel, Task>>()))
                .Throws(() => new TabExistsException { Tab = tab });

            var messageBoxManager = new Mock<IMessageBoxManager>();
            messageBoxManager
                .Setup(m => m.ShowCustomMessageBoxAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<Icon>(), It.IsAny<Window>(), It.IsAny<ButtonDefinition[]>()))
                .ReturnsAsync(messageBoxResult);

            var viewModel = CreateViewModel(tabManager.Object, null, fileManager.Object, messageBoxManager.Object);

            // Act

            await viewModel.OpenFileCommand.Execute();

            // Assert

            fileManager.Verify(m => m.OpenFilesAsync(viewModel.View.StorageProvider), Times.Once);
            messageBoxManager.Verify(m =>
                m.ShowCustomMessageBoxAsync("Warning", $"File '{file.FileName}' is already open", Icon.Warning,
                    viewModel.View, Buttons.ReopenButton, Buttons.SkipButton), Times.Once);
            tabManager.Verify(
                m => m.CreateTab(It.IsAny<FileModel>(), It.IsAny<Func<IFileTabViewModel, Task>>(),
                    It.IsAny<Func<IFileTabViewModel, Task>>()),
                Times.Once);
            Assert.That(file.Text, shouldUpdate ? Is.EqualTo(newFile.Text) : Is.EqualTo(DefaultFile.Text));
        });
    }

    [Test]
    public async Task OpenFileAbortTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(m => m.OpenFilesAsync(It.IsAny<IStorageProvider>()))
                .ReturnsAsync(Array.Empty<FileModel>());

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>());

            var viewModel = CreateViewModel(tabManager.Object, null, fileManager.Object);

            // Act

            await viewModel.OpenFileCommand.Execute();

            // Assert

            fileManager.Verify(m => m.OpenFilesAsync(viewModel.View.StorageProvider), Times.Once);
            tabManager.Verify(
                m => m.CreateTab(It.IsAny<FileModel>(), It.IsAny<Func<IFileTabViewModel, Task>>(),
                    It.IsAny<Func<IFileTabViewModel, Task>>()),
                Times.Never);
        });
    }

    [Test]
    public async Task SaveFileTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var file = DefaultFile;
            var tab = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var fileManager = new Mock<IFileManager>();

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tab).Returns(tab);
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>());

            var viewModel = CreateViewModel(tabManager.Object, null, fileManager.Object);

            // Act

            await viewModel.SaveFileCommand.Execute(false);

            // Assert

            fileManager.Verify(m => m.WriteFileAsync(file), Times.Once);
            tabManager.Verify(m => m.UpdateHeader(tab), Times.Once);
            tabManager.Verify(m => m.UpdateForeground(tab), Times.Once);
        });
    }

    [Test]
    public async Task SaveFileAsTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            const string newPath = "new-test.asm";
            var file = DefaultFile;
            var tab = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(m => m.GetFileAsync(It.IsAny<IStorageProvider>(), It.IsAny<PickerOptions>()))
                .ReturnsAsync(newPath);

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tab).Returns(tab);
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>());

            var viewModel = CreateViewModel(tabManager.Object, null, fileManager.Object);

            // Act

            await viewModel.SaveFileCommand.Execute(true);

            // Assert

            Assert.That(file.FilePath, Is.EqualTo(newPath));
            fileManager.Verify(m => m.GetFileAsync(viewModel.View.StorageProvider, It.IsAny<PickerOptions>()),
                Times.Once);
            fileManager.Verify(m => m.WriteFileAsync(file), Times.Once);
            tabManager.Verify(m => m.UpdateHeader(tab), Times.Once);
            tabManager.Verify(m => m.UpdateForeground(tab), Times.Once);
        });
    }

    [Test]
    public async Task SaveFileAsAlreadyExistsTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var file1 = DefaultFile;
            var tab1 = new FileTabViewModel(new FileTab(), file1, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var file2 = DefaultFile;
            file2.FilePath = "another.asm";
            var tab2 = new FileTabViewModel(new FileTab(), file2, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(m => m.GetFileAsync(It.IsAny<IStorageProvider>(), It.IsAny<PickerOptions>()))
                .ReturnsAsync(file2.FilePath);

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tab).Returns(tab1);
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>(new[] { tab1, tab2 }));

            var messageBoxManager = new Mock<IMessageBoxManager>();
            messageBoxManager
                .Setup(m => m.ShowErrorMessageBox(It.IsAny<string>(), It.IsAny<Window>()))
                .Throws<NotSupportedException>(); // need for ending loop in SaveFileAsAsync

            var viewModel = CreateViewModel(tabManager.Object, null, fileManager.Object, messageBoxManager.Object);

            // Act

            var exception =
                Assert.ThrowsAsync<UnhandledErrorException>(async () => await viewModel.SaveFileCommand.Execute(true));

            // Assert

            Assert.That(exception, Has.InnerException.With.TypeOf<NotSupportedException>());
            messageBoxManager.Verify(m => m.ShowErrorMessageBox("That file already opened", viewModel.View),
                Times.Once);
        });
    }

    [Test]
    public async Task SaveProjectFileTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var file = DefaultFile;
            file.Text =
                "{\"Files\":[\"main.asm\"],\"Executable\": \"main.asm\",\"Devices\":[],\"StackAddress\":\"0o1000\",\"ProgramAddress\":\"0o1000\"}";
            var tab = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var fileManager = new Mock<IFileManager>();

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tab).Returns(tab);
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>());

            var projectManager = new Mock<IProjectManager>();
            projectManager.Setup(m => m.Project).Returns(new Project { ProjectFile = file.FilePath });
            projectManager.Setup(m => m.IsOpened).Returns(true);

            var viewModel = CreateViewModel(tabManager.Object, projectManager.Object, fileManager.Object);

            // Act

            await viewModel.SaveFileCommand.Execute(false);

            // Assert

            fileManager.Verify(m => m.WriteFileAsync(file), Times.Once);
            projectManager.Verify(m => m.ReloadProjectAsync(), Times.Once);
            tabManager.Verify(m => m.UpdateForeground(tab), Times.Once);
            tabManager.Verify(m => m.UpdateHeader(tab), Times.Once);
        });
    }

    [Test]
    public async Task SaveInvalidProjectFileTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var file = DefaultFile;
            file.Text =
                "{\"Files\":[\"main.asm\"],\"Executable\": \"main.asm\",\"Devices\":[],\"StackAddress\":\"0o1000\",\"ProgramAddress\":\"0o1000\"";
            var tab = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var error = await JsonHelper.ValidateJsonAsync<ProjectDto>(file.Text);

            var fileManager = new Mock<IFileManager>();

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tab).Returns(tab);
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>());

            var projectManager = new Mock<IProjectManager>();
            projectManager.Setup(m => m.Project).Returns(new Project { ProjectFile = file.FilePath });
            projectManager.Setup(m => m.IsOpened).Returns(true);

            var messageBoxManager = new Mock<IMessageBoxManager>();

            var viewModel = CreateViewModel(tabManager.Object, projectManager.Object, fileManager.Object,
                messageBoxManager.Object);

            // Act

            await viewModel.SaveFileCommand.Execute(true);

            // Assert

            messageBoxManager.Verify(m => m.ShowErrorMessageBox(error, viewModel.View), Times.Once);
            fileManager.Verify(m => m.WriteFileAsync(file), Times.Never);
        });
    }

    [Test]
    public async Task SaveProjectFileAsTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            const string newPath = "new-test.asm";
            var file = DefaultFile;
            var tab = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(m => m.GetFileAsync(It.IsAny<IStorageProvider>(), It.IsAny<PickerOptions>()))
                .ReturnsAsync(newPath);

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tab).Returns(tab);
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>());

            var projectManager = new Mock<IProjectManager>();
            projectManager.Setup(m => m.Project).Returns(new Project { ProjectFile = file.FilePath });
            projectManager.Setup(m => m.IsOpened).Returns(true);

            var messageBoxManager = new Mock<IMessageBoxManager>();

            var viewModel = CreateViewModel(tabManager.Object, projectManager.Object, fileManager.Object,
                messageBoxManager.Object);

            // Act

            await viewModel.SaveFileCommand.Execute(true);

            // Assert

            messageBoxManager.Verify(
                m => m.ShowErrorMessageBox("This feature is not available for project file", viewModel.View),
                Times.Once);
            fileManager.Verify(m => m.WriteFileAsync(file), Times.Never);
        });
    }

    [Test]
    public async Task SaveAllFilesTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var file = DefaultFile;

            var tabs = new[]
            {
                new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask),
                new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask)
            };

            var fileManager = new Mock<IFileManager>();

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tab).Returns(tabs[0]);
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>(tabs));

            var viewModel = CreateViewModel(tabManager.Object, null, fileManager.Object);

            // Act

            await viewModel.SaveAllFilesCommand.Execute();

            // Assert

            fileManager.Verify(m => m.WriteFileAsync(file), Times.Exactly(2));
            foreach (var tab in tabs)
            {
                tabManager.Verify(m => m.UpdateHeader(tab), Times.Once);
                tabManager.Verify(m => m.UpdateForeground(tab), Times.Once);
            }
        });
    }

    [Test]
    public async Task CloseFileTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var file = DefaultFile;
            var tab = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var fileManager = new Mock<IFileManager>();

            Func<IFileTabViewModel, Task> closeCommand = null;

            var tabManager = new Mock<ITabManager>();
            tabManager
                .Setup(m => m.CreateTab(It.IsAny<FileModel>(), It.IsAny<Func<IFileTabViewModel, Task>>(),
                    It.IsAny<Func<IFileTabViewModel, Task>>()))
                .Callback<FileModel, Func<IFileTabViewModel, Task>, Func<IFileTabViewModel, Task>>((_, _, close) =>
                {
                    closeCommand = close;
                })
                .Returns(tab);

            var viewModel = CreateViewModel(tabManager.Object, null, fileManager.Object);
            await viewModel.CreateFileCommand.Execute();

            // Act

            await closeCommand(tab);

            // Assert

            tabManager.VerifyRemove(m => m.DeleteTab(tab), Times.Once);
        });
    }

    [Test]
    [TestCase(ButtonResult.Yes)]
    [TestCase(ButtonResult.No)]
    public async Task CloseFileNeedSaveTest(ButtonResult messageBoxResult)
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var file = DefaultFile;
            file.IsNeedSave = true;
            var tab = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var fileManager = new Mock<IFileManager>();

            Func<IFileTabViewModel, Task> closeCommand = null;

            var tabManager = new Mock<ITabManager>();
            tabManager
                .Setup(m => m.CreateTab(It.IsAny<FileModel>(), It.IsAny<Func<IFileTabViewModel, Task>>(),
                    It.IsAny<Func<IFileTabViewModel, Task>>()))
                .Callback<FileModel, Func<IFileTabViewModel, Task>, Func<IFileTabViewModel, Task>>((_, _, close) =>
                {
                    closeCommand = close;
                })
                .Returns(tab);

            var messageBoxManager = new Mock<MessageBoxManager>();
            messageBoxManager
                .Setup(m => m.ShowMessageBoxAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ButtonEnum>(),
                    It.IsAny<Icon>(), It.IsAny<Window>()))
                .ReturnsAsync(messageBoxResult);

            var viewModel = CreateViewModel(tabManager.Object, null, fileManager.Object, messageBoxManager.Object);
            await viewModel.CreateFileCommand.Execute();

            // Act

            await closeCommand(tab);

            // Assert

            tabManager.VerifyRemove(m => m.DeleteTab(tab), Times.Once);
            messageBoxManager.Verify(m => m.ShowMessageBoxAsync("Confirmation",
                    $"Do you want to save the file '{file.FileName}'?", ButtonEnum.YesNo, Icon.Question,
                    viewModel.View),
                Times.Once);

            fileManager.Verify(m => m.WriteFileAsync(file),
                messageBoxResult == ButtonResult.Yes ? Times.Once : Times.Never);
        });
    }

    [Test]
    public async Task CloseProjectFileTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var file = DefaultFile;
            var tab = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var fileManager = new Mock<IFileManager>();

            Func<IFileTabViewModel, Task> closeCommand = null;

            var tabManager = new Mock<ITabManager>();
            tabManager
                .Setup(m => m.CreateTab(It.IsAny<FileModel>(), It.IsAny<Func<IFileTabViewModel, Task>>(),
                    It.IsAny<Func<IFileTabViewModel, Task>>()))
                .Callback<FileModel, Func<IFileTabViewModel, Task>, Func<IFileTabViewModel, Task>>((_, _, close) =>
                {
                    closeCommand = close;
                })
                .Returns(tab);

            var projectManager = new Mock<IProjectManager>();
            projectManager.Setup(m => m.Project).Returns(new Project { ProjectFile = file.FilePath });
            projectManager.Setup(m => m.IsOpened).Returns(true);

            var messageBoxManager = new Mock<IMessageBoxManager>();

            var viewModel = CreateViewModel(tabManager.Object, projectManager.Object, fileManager.Object,
                messageBoxManager.Object);
            await viewModel.CreateFileCommand.Execute();

            // Act

            await closeCommand(tab);

            // Assert

            tabManager.VerifyRemove(m => m.DeleteTab(tab), Times.Never);
            messageBoxManager.Verify(
                m => m.ShowErrorMessageBox("Cannot close project file", viewModel.View),
                Times.Once);
        });
    }

    [Test]
    [TestCase(ButtonResult.Yes)]
    [TestCase(ButtonResult.No)]
    public async Task DeleteFileTest(ButtonResult messageBoxResult)
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var file = DefaultFile;
            var tab = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var fileManager = new Mock<IFileManager>();

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tab).Returns(tab);
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>());

            var projectManager = new Mock<IProjectManager>();
            var messageBoxManager = new Mock<IMessageBoxManager>();

            var viewModel = CreateViewModel(tabManager.Object, null, fileManager.Object);

            // Act

            await viewModel.DeleteFileCommand.Execute();

            // Assert

            messageBoxManager.Verify(m => m.ShowMessageBoxAsync("Confirmation",
                    $"Are you sure you want to delete the file '{file.FileName}'?",
                    ButtonEnum.YesNo, Icon.Question, viewModel.View),
                Times.Once);

            Func<Times> calledTimes = messageBoxResult == ButtonResult.Yes ? Times.Once : Times.Never;
            tabManager.Verify(m => m.DeleteTab(tab), calledTimes);
            fileManager.Verify(m => m.DeleteAsync(file), calledTimes);
            projectManager.Verify(m => m.RemoveFileFromProject(file.FilePath), calledTimes);
            projectManager.Verify(m => m.SaveProjectAsync(), calledTimes);
        });
    }

    [Test]
    public async Task DeleteProjectFileTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var file = DefaultFile;
            var tab = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var fileManager = new Mock<IFileManager>();

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tab).Returns(tab);
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>());

            var projectManager = new Mock<IProjectManager>();
            projectManager.Setup(m => m.Project).Returns(new Project { ProjectFile = file.FilePath });
            projectManager.Setup(m => m.IsOpened).Returns(true);

            var messageBoxManager = new Mock<IMessageBoxManager>();
            messageBoxManager
                .Setup(m => m.ShowErrorMessageBox(It.IsAny<string>(), It.IsAny<Window>()))
                .Throws<NotSupportedException>();

            var viewModel = CreateViewModel(tabManager.Object, null, fileManager.Object);

            // Act

            await viewModel.DeleteFileCommand.Execute();

            // Assert

            messageBoxManager.Verify(m => m.ShowErrorMessageBox("Cannot delete project file", viewModel.View),
                Times.Once);
            tabManager.Verify(m => m.DeleteTab(tab), Times.Never);
            fileManager.Verify(m => m.DeleteAsync(file), Times.Never);
        });
    }

    [Test]
    public async Task OpenSettingsWindowTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var windowProvider = new Mock<IWindowProvider>();

            var viewModel = CreateViewModel(windowProvider: windowProvider.Object);

            // Act

            await viewModel.OpenSettingsWindowCommand.Execute();

            // Assert

            windowProvider.Verify(
                m => m.CreateWindow<SettingsWindow, SettingsViewModel>(It.IsAny<ProjectManager>(), It.IsAny<FileManager>()),
                Times.Once);
        });
    }

    [Test]
    public async Task TabsChangedTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var tabManager = new TabManager();

            var viewModel = CreateViewModel(tabManager: tabManager);
            var propertyChangedAssert = new PropertyChangedAssert(viewModel);

            // Act

            tabManager.CreateTab(new FileModel(), _ => Task.CompletedTask, _ => Task.CompletedTask);

            // Assert

            propertyChangedAssert.Assert(nameof(viewModel.Tabs));
        });
    }

    [Test]
    public async Task TabChangedTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var tabManager = new TabManager();
            var tab = tabManager.CreateTab(new FileModel(), _ => Task.CompletedTask, _ => Task.CompletedTask);

            var viewModel = CreateViewModel(tabManager: tabManager);
            var propertyChangedAssert = new PropertyChangedAssert(viewModel);

            // Act

            tabManager.SelectTab(tab);

            // Assert

            propertyChangedAssert.Assert(nameof(viewModel.FileContent));
        });
    }

    [Test]
    public async Task ProjectChangedTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var file = DefaultFile;
            var tab = new FileTabViewModel(new FileTab(), file, _ => Task.CompletedTask, _ => Task.CompletedTask);

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(m => m.OpenFileAsync(It.IsAny<string>()))
                .ReturnsAsync(file);

            var tabManager = new Mock<ITabManager>();
            tabManager.Setup(m => m.Tabs).Returns(new ObservableCollection<IFileTabViewModel>(new[] { tab }));

            var projectManager = new Mock<IProjectManager>();
            projectManager.Setup(m => m.Project).Returns(new Project { ProjectFile = file.FilePath });
            projectManager.Setup(m => m.IsOpened).Returns(true);

            var viewModel = CreateViewModel(tabManager.Object, projectManager.Object, fileManager.Object);

            // Act

            projectManager.RaiseAsync(m => m.PropertyChanged += null, projectManager.Object,
                new PropertyChangedEventArgs(nameof(projectManager.Object.Project)));

            // Assert

            fileManager.Verify(m => m.OpenFileAsync(file.FilePath), Times.Once);
        });
    }

    [Test]
    public async Task InitWindowCreateProjectTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            const string projectName = "project";
            var directory = Directory.GetCurrentDirectory();
            var mainFile = $"{directory}{Path.DirectorySeparatorChar}main.asm";
            var projectFile = $"{directory}{Path.DirectorySeparatorChar}{projectName}.pdp11proj";

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(m => m.OpenFileAsync(It.IsAny<string>()))
                .ReturnsAsync(new FileModel { FilePath = $"{Guid.NewGuid()}" });

            var projectManager = new Mock<IProjectManager>();
            projectManager
                .Setup(m => m.CreateProjectAsync(It.IsAny<IStorageProvider>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            projectManager.Setup(m => m.Project).Returns(new Project
            {
                ProjectFile = projectFile,
                Files = new List<string> { mainFile }
            });

            var messageBoxManager = new Mock<IMessageBoxManager>();
            messageBoxManager
                .Setup(m => m.ShowCustomMessageBoxAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<Icon>(), It.IsAny<Window>(), It.IsAny<ButtonDefinition[]>()))
                .ReturnsAsync(Buttons.CreateButton.Name);
            messageBoxManager
                .Setup(m => m.ShowInputMessageBoxAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ButtonEnum>(),
                    It.IsAny<Icon>(), It.IsAny<Window>(), It.IsAny<string>()))
                .ReturnsAsync((ButtonResult.Ok, projectName));

            var viewModel = CreateViewModel(null, projectManager.Object, fileManager.Object, messageBoxManager.Object);

            // Act

            var isShown = false;

            viewModel.Show();
            viewModel.View.Opened += (_, _) => { isShown = true; };

            // Assert

            await TaskHelper.WaitForCondition(() => isShown, TimeSpan.FromSeconds(10));

            messageBoxManager
                .Verify(
                    m => m.ShowCustomMessageBoxAsync("Init", "Create or open project", Icon.Info,
                        viewModel.View, Buttons.CreateButton, Buttons.OpenButton, Buttons.CancelButton
                    ), Times.Once);
            messageBoxManager
                .Verify(
                    m => m.ShowInputMessageBoxAsync("Create project", "Enter project name", ButtonEnum.OkCancel,
                        Icon.Setting, viewModel.View, "Project name"),
                    Times.Once);

            projectManager.Verify(m => m.CreateProjectAsync(viewModel.View.StorageProvider, projectName), Times.Once);
            projectManager.Verify(m => m.AddFileToProject(mainFile), Times.Once);
            projectManager.Verify(m => m.SetExecutableFile(mainFile), Times.Once);
            projectManager.Verify(m => m.SaveProjectAsync(), Times.Once);

            fileManager.Verify(m => m.WriteFileAsync(It.Is<FileModel>(f => f.FilePath == mainFile)), Times.Once);
            fileManager.Verify(m => m.OpenFileAsync(projectFile), Times.Once);
            fileManager.Verify(m => m.OpenFileAsync(mainFile), Times.Once);
        });
    }

    [Test]
    public async Task InitWindowOpenProjectTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            const string projectName = "project";
            var directory = Directory.GetCurrentDirectory();
            var mainFile = $"{directory}{Path.DirectorySeparatorChar}main.asm";
            var projectFile = $"{directory}{Path.DirectorySeparatorChar}{projectName}.pdp11proj";

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(m => m.OpenFileAsync(It.IsAny<string>()))
                .ReturnsAsync(new FileModel { FilePath = $"{Guid.NewGuid()}" });

            var projectManager = new Mock<IProjectManager>();
            projectManager
                .Setup(m => m.OpenProjectAsync(It.IsAny<IStorageProvider>()))
                .ReturnsAsync(true);
            projectManager.Setup(m => m.Project).Returns(new Project
            {
                ProjectFile = projectFile,
                Files = new List<string> { mainFile }
            });

            var messageBoxManager = new Mock<IMessageBoxManager>();
            messageBoxManager
                .Setup(m => m.ShowCustomMessageBoxAsync(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<Icon>(), It.IsAny<Window>(), It.IsAny<ButtonDefinition[]>()))
                .ReturnsAsync(Buttons.OpenButton.Name);

            var viewModel = CreateViewModel(null, projectManager.Object, fileManager.Object, messageBoxManager.Object);

            // Act

            var isShown = false;

            viewModel.Show();
            viewModel.View.Opened += (_, _) => { isShown = true; };

            // Assert

            await TaskHelper.WaitForCondition(() => isShown, TimeSpan.FromSeconds(10));

            messageBoxManager
                .Verify(
                    m => m.ShowCustomMessageBoxAsync("Init", "Create or open project", Icon.Info,
                        viewModel.View, Buttons.CreateButton, Buttons.OpenButton, Buttons.CancelButton
                    ), Times.Once);

            projectManager.Verify(m => m.OpenProjectAsync(viewModel.View.StorageProvider), Times.Once);
            fileManager.Verify(m => m.OpenFileAsync(projectFile), Times.Once);
            fileManager.Verify(m => m.OpenFileAsync(mainFile), Times.Once);
        });
    }

    [Test]
    public async Task InitWindowOpenProjectFromParamTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            const string projectName = "project";
            var directory = Directory.GetCurrentDirectory();
            var mainFile = $"{directory}{Path.DirectorySeparatorChar}main.asm";
            var projectFile = $"{directory}{Path.DirectorySeparatorChar}{projectName}.pdp11proj";
            SettingsManager.Instance.CommandLineOptions.Project = projectFile;

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(m => m.OpenFileAsync(It.IsAny<string>()))
                .ReturnsAsync(new FileModel { FilePath = $"{Guid.NewGuid()}" });

            var projectManager = new Mock<IProjectManager>();
            projectManager
                .Setup(m => m.LoadProjectAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            projectManager.Setup(m => m.Project).Returns(new Project
            {
                ProjectFile = projectFile,
                Files = new List<string> { mainFile }
            });

            var messageBoxManager = new Mock<IMessageBoxManager>();

            var viewModel = CreateViewModel(null, projectManager.Object, fileManager.Object, messageBoxManager.Object);

            // Act

            var isShown = false;

            viewModel.Show();
            viewModel.View.Opened += (_, _) => { isShown = true; };

            // Assert

            await TaskHelper.WaitForCondition(() => isShown, TimeSpan.FromSeconds(10));

            projectManager.Verify(m => m.LoadProjectAsync(projectFile), Times.Once);
            fileManager.Verify(m => m.OpenFileAsync(projectFile), Times.Once);
            fileManager.Verify(m => m.OpenFileAsync(mainFile), Times.Once);
        });
    }

    [Test]
    public async Task OpenProjectConfirmForClosingTabs()
    {
        await NewProjectConfirmClosingTabsTest(async viewModel => await viewModel.OpenProjectCommand.Execute());
    }

    [Test]
    public async Task CreateProjectConfirmForClosingTabs()
    {
        await NewProjectConfirmClosingTabsTest(async viewModel => await viewModel.CreateProjectCommand.Execute());
    }

    [Test]
    public async Task InitWindowOpenProjectFromInvalidParamTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            SettingsManager.Instance.CommandLineOptions.Project = string.Empty;

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(m => m.OpenFileAsync(It.IsAny<string>()))
                .ReturnsAsync(new FileModel { FilePath = $"{Guid.NewGuid()}" });

            var projectManager = new Mock<IProjectManager>();
            projectManager
                .Setup(m => m.LoadProjectAsync(It.IsAny<string>()))
                .Throws(() => new InvalidOperationException("invalid"));
            projectManager
                .Setup(m => m.OpenProjectAsync(It.IsAny<IStorageProvider>()))
                .Throws<InvalidOperationException>();

            var messageBoxManager = new Mock<IMessageBoxManager>();

            var viewModel = CreateViewModel(null, projectManager.Object, fileManager.Object, messageBoxManager.Object);

            // Act

            var isShown = false;
            viewModel.Show();
            viewModel.View.Opened += (_, _) => { isShown = true; };

            // Assert

            await TaskHelper.WaitForCondition(() => isShown, TimeSpan.FromSeconds(10));

            projectManager.Verify(m => m.LoadProjectAsync(string.Empty), Times.Once);
            projectManager.Verify(m => m.OpenProjectAsync(viewModel.View.StorageProvider), Times.Once);
            messageBoxManager.Verify(m => m.ShowErrorMessageBox("invalid", viewModel.View), Times.Once);
        });
    }

    [Test]
    public async Task DoNotOpenProjectIfCancelPickerTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var fileManager = new Mock<IFileManager>();

            var projectManager = new Mock<IProjectManager>();
            projectManager
                .Setup(m => m.OpenProjectAsync(It.IsAny<IStorageProvider>()))
                .ReturnsAsync(false);

            var messageBoxManager = new Mock<IMessageBoxManager>();

            var viewModel = CreateViewModel(null, projectManager.Object, fileManager.Object, messageBoxManager.Object);

            // Act

            await viewModel.OpenProjectCommand.Execute();

            // Assert

            projectManager.Verify(m => m.OpenProjectAsync(viewModel.View.StorageProvider), Times.Once);
            fileManager.Verify(m => m.OpenFileAsync(It.IsAny<string>()), Times.Never);
        });
    }

    [Test]
    public async Task DoNotCreateProjectIfCancelNameInputTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var fileManager = new Mock<IFileManager>();

            var projectManager = new Mock<IProjectManager>();

            var messageBoxManager = new Mock<IMessageBoxManager>();
            messageBoxManager
                .Setup(m => m.ShowInputMessageBoxAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ButtonEnum>(),
                    It.IsAny<Icon>(), It.IsAny<Window>(), It.IsAny<string>()))
                .ReturnsAsync((ButtonResult.Cancel, ""));

            var viewModel = CreateViewModel(null, projectManager.Object, fileManager.Object, messageBoxManager.Object);

            // Act

            await viewModel.CreateProjectCommand.Execute();

            // Assert

            messageBoxManager
                .Verify(
                    m => m.ShowInputMessageBoxAsync("Create project", "Enter project name", ButtonEnum.OkCancel,
                        Icon.Setting, viewModel.View, "Project name"),
                    Times.Once);
            projectManager.Verify(m => m.CreateProjectAsync(It.IsAny<IStorageProvider>(), It.IsAny<string>()),
                Times.Never);
            fileManager.Verify(m => m.OpenFileAsync(It.IsAny<string>()), Times.Never);
        });
    }

    [Test]
    public async Task DoNotCreateProjectIfCancelPickerTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var fileManager = new Mock<IFileManager>();

            var projectManager = new Mock<IProjectManager>();
            projectManager
                .Setup(m => m.CreateProjectAsync(It.IsAny<IStorageProvider>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var messageBoxManager = new Mock<IMessageBoxManager>();
            messageBoxManager
                .Setup(m => m.ShowInputMessageBoxAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ButtonEnum>(),
                    It.IsAny<Icon>(), It.IsAny<Window>(), It.IsAny<string>()))
                .ReturnsAsync((ButtonResult.Ok, ""));

            var viewModel = CreateViewModel(null, projectManager.Object, fileManager.Object, messageBoxManager.Object);

            // Act

            await viewModel.CreateProjectCommand.Execute();

            // Assert

            messageBoxManager
                .Verify(
                    m => m.ShowInputMessageBoxAsync("Create project", "Enter project name", ButtonEnum.OkCancel,
                        Icon.Setting, viewModel.View, "Project name"),
                    Times.Once);
            projectManager.Verify(m => m.CreateProjectAsync(viewModel.View.StorageProvider, ""), Times.Once);
            fileManager.Verify(m => m.OpenFileAsync(It.IsAny<string>()), Times.Never);
        });
    }

    [Test]
    public async Task RepeatNameInputIfInvalidTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var fileManager = new Mock<IFileManager>();

            var projectManager = new Mock<IProjectManager>();
            projectManager
                .Setup(m => m.CreateProjectAsync(It.IsAny<IStorageProvider>(), It.IsAny<string>()))
                .Throws<ArgumentException>();

            var results = new[] { (ButtonResult.Ok, ""), (ButtonResult.Cancel, "") };
            var count = 0;
            var messageBoxManager = new Mock<IMessageBoxManager>();
            messageBoxManager
                .Setup(m => m.ShowInputMessageBoxAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ButtonEnum>(),
                    It.IsAny<Icon>(), It.IsAny<Window>(), It.IsAny<string>()))
                .ReturnsAsync(() => results[count++]);

            var viewModel = CreateViewModel(null, projectManager.Object, fileManager.Object, messageBoxManager.Object);

            // Act

            await viewModel.CreateProjectCommand.Execute();

            // Assert

            messageBoxManager
                .Verify(
                    m => m.ShowInputMessageBoxAsync("Create project", "Enter project name", ButtonEnum.OkCancel,
                        Icon.Setting, viewModel.View, "Project name"),
                    Times.Exactly(2));
            messageBoxManager.Verify(m => m.ShowErrorMessageBox(It.IsAny<string>(), viewModel.View), Times.Once);
            projectManager.Verify(m => m.CreateProjectAsync(It.IsAny<IStorageProvider>(), It.IsAny<string>()),
                Times.Once);
            fileManager.Verify(m => m.OpenFileAsync(It.IsAny<string>()), Times.Never);
        });
    }

    private Task NewProjectConfirmClosingTabsTest(Func<IMainWindowViewModel, Task> action)
    {
        return RunTest(() =>
        {
            // Arrange

            var tabManager = new Mock<ITabManager>();
            tabManager
                .Setup(m => m.Tabs)
                .Returns(new ObservableCollection<IFileTabViewModel>(new[] { Mock.Of<IFileTabViewModel>() }));

            var messageBoxManager = new Mock<IMessageBoxManager>();
            messageBoxManager
                .Setup(m => m.ShowMessageBoxAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ButtonEnum>(),
                    It.IsAny<Icon>(), It.IsAny<Window>()))
                .Throws<InvalidOperationException>();

            var viewModel = CreateViewModel(tabManager.Object, null, null, messageBoxManager.Object);

            // Act & Assert

            var ex = Assert.CatchAsync<UnhandledErrorException>(async () => await action(viewModel));
            Assert.That(ex!.InnerException, Is.TypeOf<InvalidOperationException>());
            messageBoxManager
                .Verify(m => m.ShowMessageBoxAsync("Warning", "This action closes current project and all tabs",
                    ButtonEnum.OkAbort, Icon.Warning, viewModel.View), Times.Once);
        });
    }

    private static MainWindowViewModel CreateViewModel(ITabManager tabManager = null,
        IProjectManager projectManager = null, IFileManager fileManager = null,
        IMessageBoxManager messageBoxManager = null, IWindowProvider windowProvider = null) =>
        new(new MainWindow(),
            tabManager ?? new TabManager(),
            projectManager ?? new ProjectManager(new ProjectProvider(), new DeviceValidator(new DeviceProvider())),
            fileManager ?? new FileManager(),
            messageBoxManager ?? new MessageBoxManager(),
            windowProvider ?? new WindowProvider()
        );
}