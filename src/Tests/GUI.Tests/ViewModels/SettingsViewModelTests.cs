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

namespace GUI.Tests.ViewModels;

public class SettingsViewModelTests : GuiTest<App>
{
    [SetUp]
    public void SetUp()
    {
        SettingsManager.Create(new EditorOptions(), new CommandLineOptions());
    }

    [Test]
    public void CreationTest()
    {
        RunTest(() =>
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
    public void AddDeviceTest()
    {
        RunTest(() =>
        {
            // Arrange

            const string devicePath = "a.dll";
            PickerOptions options = null;

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(m => m.GetFileAsync(It.IsAny<IStorageProvider>(), It.IsAny<PickerOptions>()))
                .Callback<IStorageProvider, PickerOptions>((_, ops) => { options = ops; })
                .ReturnsAsync(devicePath);

            var projectManager = new Mock<IProjectManager>();
            projectManager.Setup(m => m.SaveProjectAsync()).Returns(Task.CompletedTask);

            var viewModel =
                new SettingsViewModel(new SettingsWindow(), projectManager.Object, fileManager.Object);

            // Act

            viewModel.AddDeviceCommand.Execute(null);

            // Assert

            fileManager.Verify(m => m.GetFileAsync(It.IsAny<IStorageProvider>(), options), Times.Once);
            projectManager.Verify(m => m.AddDeviceToProject(devicePath), Times.Once);
            projectManager.Verify(m => m.SaveProjectAsync(), Times.Once);

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
    public void DeleteDevicesTest()
    {
        RunTest(() =>
        {
            // Arrange

            var devices = new[] { "a.dll", "b.dll" };

            var projectManager = new Mock<IProjectManager>();
            projectManager.Setup(m => m.SaveProjectAsync()).Returns(Task.CompletedTask);

            var viewModel = new SettingsViewModel(new SettingsWindow(), projectManager.Object, null)
            {
                SelectedDevices = new ObservableCollection<string>(devices)
            };

            // Act

            viewModel.DeleteDeviceCommand.Execute(null);

            // Assert

            projectManager.Verify(m => m.RemoveDeviceFromProject(devices[0]), Times.Once);
            projectManager.Verify(m => m.RemoveDeviceFromProject(devices[1]), Times.Once);
            projectManager.Verify(m => m.SaveProjectAsync(), Times.Once);
        });
    }

    [Test]
    public void ChangeFontTest()
    {
        RunTest(() =>
        {
            // Arrange

            var fontFamily = new FontFamily("Font");
            const int fontSize = 16;

            var viewModel = new SettingsViewModel(new SettingsWindow(), new ProjectManager(new ProjectProvider()), null)
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
    public void DeviceChangedEvent()
    {
        RunAsyncTest(async () =>
        {
            // Arrange

            var projectManager = new Mock<IProjectManager>();
            var viewModel = new SettingsViewModel(new SettingsWindow(), projectManager.Object, new FileManager());
            var propertyAssert = new PropertyChangedAssert(viewModel);

            // Act

            await projectManager.RaiseAsync(m => m.PropertyChanged += null,
                projectManager.Object,
                new PropertyChangedEventArgs("Project"));

            // Assert

            projectManager.VerifyAdd(m => m.PropertyChanged += It.IsAny<PropertyChangedEventHandler>(), Times.Once);
            propertyAssert.Assert(nameof(viewModel.Devices));
        });
    }
}