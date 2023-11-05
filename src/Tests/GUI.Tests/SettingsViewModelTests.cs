using System.Collections.ObjectModel;
using System.ComponentModel;
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

            const string devicePath = "a.dll";
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

            viewModel.AddDeviceCommand.Execute(null);

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

    [Test]
    public async Task DeleteDevicesTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var devices = new[] { "a.dll", "b.dll" };

            var projectManagerMock = new Mock<IProjectManager>();
            projectManagerMock.Setup(m => m.SaveProjectAsync()).Returns(Task.CompletedTask);

            var viewModel = new SettingsViewModel(new SettingsWindow(), projectManagerMock.Object, null)
            {
                SelectedDevices = new ObservableCollection<string>(devices)
            };

            // Act

            viewModel.DeleteDeviceCommand.Execute(null);

            // Assert

            projectManagerMock.Verify(m => m.RemoveDeviceFromProject(devices[0]), Times.Once);
            projectManagerMock.Verify(m => m.RemoveDeviceFromProject(devices[1]), Times.Once);
            projectManagerMock.Verify(m => m.SaveProjectAsync(), Times.Once);
        });
    }

    [Test]
    public async Task ChangeFontTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var fontFamily = new FontFamily("Font");
            const int fontSize = 16;

            var viewModel = new SettingsViewModel(new SettingsWindow(), new ProjectManager(null), null)
            {
                // Act
                FontFamily = fontFamily,
                FontSize = fontSize
            };

            // Assert

            Assert.Multiple(() =>
            {
                Assert.That(SettingsManager.Instance.FontFamily, Is.EqualTo(fontFamily));
                Assert.That(SettingsManager.Instance.FontSize, Is.EqualTo(fontSize));
            });
        });
    }

    [Test]
    public async Task DeviceChangedEvent()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var property = string.Empty;
            var projectManagerMock = new Mock<IProjectManager>();
            var viewModel = new SettingsViewModel(new SettingsWindow(), projectManagerMock.Object, new FileManager());

            viewModel.PropertyChanged += (_, args) => { property = args.PropertyName; };

            // Act

            await projectManagerMock.RaiseAsync(m => m.PropertyChanged += null,
                projectManagerMock.Object,
                new PropertyChangedEventArgs("Project"));

            // Assert

            projectManagerMock.VerifyAdd(m => m.PropertyChanged += It.IsAny<PropertyChangedEventHandler>(), Times.Once);
            Assert.That(property, Is.EqualTo(nameof(viewModel.Devices)));
        });
    }
}