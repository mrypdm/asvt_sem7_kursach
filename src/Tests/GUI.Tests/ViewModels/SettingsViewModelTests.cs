using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Domain.Providers;
using GUI.Managers;
using GUI.Models;
using GUI.ViewModels;
using GUI.Views;
using Moq;
using Shared.Helpers;

namespace GUI.Tests.ViewModels;

public class SettingsViewModelTests : GuiTest<App>
{
    [SetUp]
    public void SetUp()
    {
        SettingsManager.Create(new EditorOptions(), new CommandLineOptions());
    }

    [Test]
    public async Task AllFontsTest()
    {
        await RunTest(() =>
        {
            // Assert

            Assert.That(SettingsManager.AllFontFamilies, Is.EqualTo(FontManager.Current.SystemFonts));
        });
    }

    [Test]
    public async Task AddDeviceTest()
    {
        await RunAsyncTest(async () =>
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

            var viewModel = CreateViewModel(projectManager.Object, fileManager.Object);

            // Act

            await viewModel.AddDeviceCommand.Execute();

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
    public async Task DeleteDevicesTest()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var devices = new[] { "a.dll", "b.dll" };

            var projectManager = new Mock<IProjectManager>();
            projectManager.Setup(m => m.SaveProjectAsync()).Returns(Task.CompletedTask);

            var viewModel = CreateViewModel(projectManager.Object);
            viewModel.SelectedDevices = new ObservableCollection<string>(devices);

            // Act

            await viewModel.DeleteDeviceCommand.Execute();

            // Assert

            projectManager.Verify(m => m.RemoveDeviceFromProject(devices[0]), Times.Once);
            projectManager.Verify(m => m.RemoveDeviceFromProject(devices[1]), Times.Once);
            projectManager.Verify(m => m.SaveProjectAsync(), Times.Once);
        });
    }

    [Test]
    public async Task DeviceChangedEvent()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            var projectManager = new Mock<IProjectManager>();
            var viewModel = CreateViewModel(projectManager.Object);
            var propertyAssert = new PropertyChangedAssert(viewModel);

            // Act

            await projectManager.RaiseAsync(m => m.PropertyChanged += null,
                projectManager.Object,
                new PropertyChangedEventArgs(nameof(projectManager.Object.Project)));

            // Assert

            projectManager.VerifyAdd(m => m.PropertyChanged += It.IsAny<PropertyChangedEventHandler>(), Times.Once);
            propertyAssert.Assert(nameof(viewModel.Devices));
        });
    }

    [Test]
    public async Task SaveOnClosing()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            const string settingsFile = "appsettings.json";

            File.Delete(settingsFile);

            var viewModel = CreateViewModel();

            // Act

            viewModel.View.Close();

            // Assert

            await TaskHelper.WaitForCondition(() => File.Exists(settingsFile), TimeSpan.FromSeconds(10));

            var options = await JsonHelper.DeserializeFileAsync<EditorOptions>(settingsFile);
            Assert.Multiple(() =>
            {
                Assert.That(options.FontFamily, Is.EqualTo(SettingsManager.Instance.FontFamily.Name));
                Assert.That(options.FontSize, Is.EqualTo(SettingsManager.Instance.FontSize));
            });
        });
    }

    private static SettingsViewModel CreateViewModel(IProjectManager projectManager = null,
        IFileManager fileManager = null) =>
        new(new SettingsWindow(),
            projectManager ?? new ProjectManager(new ProjectProvider()),
            fileManager ?? new FileManager());
}