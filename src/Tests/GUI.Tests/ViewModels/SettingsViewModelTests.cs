using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Devices.Providers;
using Devices.Validators;
using Domain.Models;
using Domain.Providers;
using GUI.Managers;
using GUI.MessageBoxes;
using GUI.Models;
using GUI.ViewModels;
using GUI.Views;
using Moq;
using Shared.Exceptions;
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
    public async Task CreationTest()
    {
        await RunTest(() =>
        {
            // Act
            var viewModel = CreateViewModel();

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
    public async Task ChangeFontTest()
    {
        await RunTest(() =>
        {
            // Arrange

            var fontFamily = new FontFamily("Font");
            const int fontSize = 16;

            var viewModel = CreateViewModel();

            // Act
            viewModel.FontFamily = fontFamily;
            viewModel.FontSize = fontSize;

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

    [Test]
    public async Task ValidateDevices()
    {
        await RunAsyncTest(async () =>
        {
            // Arrange

            const string badDevice = "badDevice.dll";
            const string goodDevice = "goodDevice.dll";

            var project = new Mock<IProject>();
            project
                .Setup(m => m.Devices)
                .Returns(new List<string> { goodDevice, badDevice });

            var projectManager = new Mock<IProjectManager>();
            projectManager
                .Setup(m => m.Project)
                .Returns(project.Object);

            var validator = new Mock<IDeviceValidator>();
            validator.Setup(m => m.ThrowIfInvalid(badDevice)).Throws(() => new ValidationException(badDevice));

            var messageBoxManager = new Mock<IMessageBoxManager>();

            var viewModel = CreateViewModel(projectManager.Object, null, validator.Object, messageBoxManager.Object);

            // Act

            await viewModel.ValidateDevicesCommand.Execute();

            // Assert

            validator.Verify(m => m.ThrowIfInvalid(goodDevice), Times.Once);
            validator.Verify(m => m.ThrowIfInvalid(badDevice), Times.Once);
            messageBoxManager.Verify(m => m.ShowErrorMessageBox(badDevice, viewModel.View), Times.Once);
        });
    }

    private static SettingsViewModel CreateViewModel(IProjectManager projectManager = null,
        IFileManager fileManager = null, IDeviceValidator validator = null,
        IMessageBoxManager messageBoxManager = null) =>
        new(new SettingsWindow(),
            projectManager ?? new ProjectManager(new ProjectProvider(), new DeviceValidator(new DeviceProvider())),
            fileManager ?? new FileManager(),
            validator ?? new DeviceValidator(new DeviceProvider()),
            messageBoxManager ?? new MessageBoxManager());
}