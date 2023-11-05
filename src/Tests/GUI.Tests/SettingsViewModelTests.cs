using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Domain.Providers;
using GUI.Managers;
using GUI.Models;
using GUI.ViewModels;
using GUI.Views;
using Moq;

namespace GUI.Tests;

public class SettingsViewModelTests : GuiTest<App>
{
    [SetUp]
    public void SetUp()
    {
        SettingsManager.Create(new EditorOptions(), new CommandLineOptions());
    }

    [Test]
    public async Task CreationTest()
    {
        await RunTest(() =>
        {
            // Act
            var viewModel = new SettingsViewModel(new SettingsWindow(), new ProjectManager(new ProjectProvider()),
                new FileManager());

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.FontFamily, Is.EqualTo(SettingsManager.Instance.FontFamily));
                Assert.That(viewModel.FontSize, Is.EqualTo(SettingsManager.Instance.FontSize));
                Assert.That(viewModel.AllFontFamilies, Is.EqualTo(FontManager.Current.SystemFonts));
            });
        });
    }

    [Test]
    public async Task AddDeviceTest()
    {
        await RunTest(() =>
        {
            // Arrange

            const string devicePath = "C:\\a.dll";
            PickerOptions options = null;

            var fileManagerMock = new Mock<IFileManager>();
            fileManagerMock
                .Setup(m => m.GetFileAsync(It.IsAny<IStorageProvider>(), It.IsAny<PickerOptions>()))
                .Callback<IStorageProvider, PickerOptions>((_, ops) => { options = ops; })
                .Returns(Task.FromResult(devicePath));

            var projectManagerMock = new Mock<IProjectManager>();
            projectManagerMock.Setup(m => m.SaveProjectAsync()).Returns(Task.CompletedTask);

            var viewModel =
                new SettingsViewModel(new SettingsWindow(), projectManagerMock.Object, fileManagerMock.Object);

            // Act

            viewModel.AddDeviceCommand.Execute(Unit.Default);

            // Assert

            fileManagerMock.Verify(m => m.GetFileAsync(It.IsAny<IStorageProvider>(), options), Times.Once);
            projectManagerMock.Verify(m => m.AddDeviceToProject(devicePath), Times.Once);
            projectManagerMock.Verify(m => m.SaveProjectAsync(), Times.Once);

            var openOptions = options as FilePickerOpenOptions;
            Assert.Multiple(() =>
            {
                Assert.That(options, Is.TypeOf<FilePickerOpenOptions>());
                Assert.That(openOptions!.AllowMultiple, Is.False);
                Assert.That(openOptions.FileTypeFilter, Has.Count.EqualTo(1));
                Assert.That(openOptions.FileTypeFilter![0].Patterns, Has.Count.EqualTo(1));
                Assert.That(openOptions.FileTypeFilter[0].Patterns![0], Is.EqualTo("*.dll"));
            });
        });
    }
}