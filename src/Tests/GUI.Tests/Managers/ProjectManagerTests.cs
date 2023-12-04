using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Domain.Extensions;
using Domain.Models;
using Domain.Providers;
using GUI.Managers;
using Moq;
using Shared.Helpers;

namespace GUI.Tests.Managers;

public class ProjectManagerTests
{
    [Test]
    public void CreationTest()
    {
        // Act

        var manager = new ProjectManager(new ProjectProvider());

        // Assert

        Assert.That(manager.IsOpened, Is.False);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var _ = manager.Project;
        });
    }

    [Test]
    public void CreateWithNullProviderTest()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var _ = new ProjectManager(null);
        });
    }

    [Test]
    public async Task CreateProjectTest()
    {
        // Arrange

        const string projectName = "TestProject";
        var directoryPath = Directory.GetCurrentDirectory();
        var expectedPath =
            $"{directoryPath}{Path.DirectorySeparatorChar}{projectName}.{ProjectManager.ProjectExtension}";

        var directory = new Mock<IStorageFolder>();
        directory.Setup(m => m.Path).Returns(new Uri(directoryPath));
        var storageProvider = new Mock<IStorageProvider>();

        storageProvider
            .Setup(m => m.OpenFolderPickerAsync(It.IsAny<FolderPickerOpenOptions>()))
            .ReturnsAsync(new[] { directory.Object });

        var manager = new ProjectManager(new ProjectProvider());
        var propertyChangedAssert = new PropertyChangedAssert(manager);

        // Act

        var res = await manager.CreateProjectAsync(storageProvider.Object, projectName);

        // Assert

        Assert.That(res, Is.True);
        Assert.That(manager.IsOpened, Is.True);
        Assert.That(manager.Project.ProjectFile, Is.EqualTo(expectedPath));
        Assert.That(PathHelper.GetPathType(manager.Project.ProjectFile), Is.EqualTo(PathHelper.PathType.File));
        storageProvider.Verify(m => m.OpenFolderPickerAsync(It.IsAny<FolderPickerOpenOptions>()), Times.Once);
        propertyChangedAssert.Assert(nameof(manager.Project));
    }

    [Test]
    public async Task CreateProjectAbortTest()
    {
        // Arrange

        var storageProvider = new Mock<IStorageProvider>();
        storageProvider
            .Setup(m => m.OpenFolderPickerAsync(It.IsAny<FolderPickerOpenOptions>()))
            .ReturnsAsync(Array.Empty<IStorageFolder>());

        var manager = new ProjectManager(new ProjectProvider());

        // Act

        var res = await manager.CreateProjectAsync(storageProvider.Object, "test");

        // Assert

        Assert.That(res, Is.False);
        Assert.That(manager.IsOpened, Is.False);
        storageProvider.Verify(m => m.OpenFolderPickerAsync(It.IsAny<FolderPickerOpenOptions>()), Times.Once);
    }

    [Test]
    public void CreateProjectInvalidNameTest()
    {
        // Arrange

        var storageProvider = new Mock<IStorageProvider>();
        var manager = new ProjectManager(new ProjectProvider());

        // Act & Assert

        Assert.ThrowsAsync<ArgumentException>(
            async () => await manager.CreateProjectAsync(storageProvider.Object, null));
    }

    [Test]
    public async Task OpenProjectTest()
    {
        // Arrange

        const string projectName = "TestProject";
        var directoryPath = Directory.GetCurrentDirectory();
        var expectedPath =
            $"{directoryPath}{Path.DirectorySeparatorChar}{projectName}.{ProjectManager.ProjectExtension}";

        var project = new Project
        {
            ProjectFile = expectedPath
        };

        await JsonHelper.SerializeToFileAsync(project, expectedPath);

        var projectProvider = new Mock<IProjectProvider>();
        projectProvider
            .Setup(m => m.OpenProjectAsync(It.IsAny<string>()))
            .ReturnsAsync(project);

        var file = new Mock<IStorageFile>();
        file.Setup(m => m.Path).Returns(new Uri(expectedPath));

        var storageProvider = new Mock<IStorageProvider>();
        storageProvider
            .Setup(m => m.OpenFilePickerAsync(It.IsAny<FilePickerOpenOptions>()))
            .ReturnsAsync(new[] { file.Object });

        var manager = new ProjectManager(projectProvider.Object);
        var propertyChangedAssert = new PropertyChangedAssert(manager);

        // Act

        var res = await manager.OpenProjectAsync(storageProvider.Object);

        // Assert

        Assert.That(res, Is.True);
        Assert.That(manager.IsOpened, Is.True);
        Assert.That(manager.Project.ProjectFile, Is.EqualTo(expectedPath));
        projectProvider.Verify(m => m.OpenProjectAsync(expectedPath), Times.Once);
        storageProvider.Verify(m => m.OpenFilePickerAsync(It.IsAny<FilePickerOpenOptions>()), Times.Once);
        file.Verify(m => m.Path, Times.Once);
        propertyChangedAssert.Assert(nameof(manager.Project));
    }

    [Test]
    public async Task OpenProjectAbortTest()
    {
        var storageProvider = new Mock<IStorageProvider>();
        storageProvider
            .Setup(m => m.OpenFilePickerAsync(It.IsAny<FilePickerOpenOptions>()))
            .ReturnsAsync(Array.Empty<IStorageFile>());

        var manager = new ProjectManager(new ProjectProvider());

        // Act

        var res = await manager.OpenProjectAsync(storageProvider.Object);

        // Assert

        Assert.That(res, Is.False);
        Assert.That(manager.IsOpened, Is.False);
    }

    [Test]
    public async Task LoadProjectTest()
    {
        // Arrange

        var (manager, projectProvider, project) = await InitMockManager();
        var propertyChangedAssert = new PropertyChangedAssert(manager);

        // Act

        await manager.LoadProjectAsync(project.ProjectFile);

        // Assert

        Assert.That(manager.IsOpened, Is.True);
        Assert.That(manager.Project.ProjectFile, Is.EqualTo(project.ProjectFile));
        projectProvider.Verify(m => m.OpenProjectAsync(project.ProjectFile), Times.Once);
        propertyChangedAssert.Assert(nameof(manager.Project));
    }

    [Test]
    public async Task ReloadProjectTest()
    {
        // Arrange

        var (manager, _, project) = await InitManager();
        await manager.LoadProjectAsync(project.ProjectFile);
        var propertyChangedAssert = new PropertyChangedAssert(manager);

        // Act

        project = new Project
        {
            Executable = PathHelper.Combine(project.ProjectDirectory, "main.asm"),
            ProjectFile = project.ProjectFile
        };
        await project.ToJsonAsync();
        await manager.ReloadProjectAsync();

        // Assert

        Assert.That(manager.Project.Executable, Is.Not.EqualTo(string.Empty));
        Assert.That(manager.Project.Executable, Is.EqualTo(project.Executable));
        propertyChangedAssert.Assert(nameof(manager.Project));
    }

    [Test]
    public async Task SaveProjectTest()
    {
        // Arrange

        var (manager, provider, project) = await InitManager();
        await manager.LoadProjectAsync(project.ProjectFile);
        const string expectedFile = "main.asm";
        var propertyChangedAssert = new PropertyChangedAssert(manager);

        // Act

        manager.AddFileToProject(expectedFile);
        await manager.SaveProjectAsync();

        // Assert

        project = await provider.OpenProjectAsync(project.ProjectFile);
        
        Assert.That(project.Files, Has.Exactly(1).Matches<string>(s => s.Contains(expectedFile)));
        propertyChangedAssert.Assert(nameof(manager.Project));
    }

    [Test]
    public async Task AddFileTest()
    {
        // Arrange

        var (manager, _, project) = await InitManager();
        await manager.LoadProjectAsync(project.ProjectFile);
        const string expectedFile = "main.asm";
        var propertyChangedAssert = new PropertyChangedAssert(manager);

        // Act

        manager.AddFileToProject(expectedFile);

        // Assert

        Assert.That(manager.Project.Files, Has.Exactly(1).Matches<string>(s => s.Contains(expectedFile)));
        propertyChangedAssert.Assert(nameof(project.Files));
    }

    [Test]
    public async Task AddDuplicateFileTest()
    {
        // Arrange

        var (manager, _, project) = await InitManager();
        await manager.LoadProjectAsync(project.ProjectFile);
        const string expectedFile = "main.asm";
        manager.AddFileToProject(expectedFile);

        // Act

        manager.AddFileToProject(expectedFile);

        // Assert

        Assert.That(manager.Project.Files, Has.Exactly(1).Matches<string>(s => s.Contains(expectedFile)));
    }

    [Test]
    public async Task RemoveFileTest()
    {
        // Arrange

        var (manager, _, project) = await InitManager();
        await manager.LoadProjectAsync(project.ProjectFile);
        const string expectedFile = "main.asm";
        manager.AddFileToProject(expectedFile);
        var propertyChangedAssert = new PropertyChangedAssert(manager);

        // Act

        manager.RemoveFileFromProject(expectedFile);

        // Assert

        Assert.That(manager.Project.Files, Does.Not.Contain(expectedFile));
        propertyChangedAssert.Assert(nameof(project.Files));
    }

    [Test]
    public async Task RemoveUnExistingFileTest()
    {
        // Arrange

        var (manager, _, project) = await InitManager();
        await manager.LoadProjectAsync(project.ProjectFile);
        const string expectedFile = "main.asm";

        // Act

        manager.RemoveFileFromProject(expectedFile);

        // Assert

        Assert.That(manager.Project.Files, Does.Not.Contain(expectedFile));
    }

    [Test]
    public async Task AddDeviceTest()
    {
        // Arrange

        var (manager, _, project) = await InitManager();
        await manager.LoadProjectAsync(project.ProjectFile);
        const string expectedFile = "main.asm";
        var propertyChangedAssert = new PropertyChangedAssert(manager);

        // Act

        manager.AddDeviceToProject(expectedFile);

        // Assert

        Assert.That(manager.Project.Devices, Does.Contain(PathHelper.GetFullPath(expectedFile)));
        propertyChangedAssert.Assert(nameof(project.Devices));
    }

    [Test]
    public async Task AddDuplicateDeviceTest()
    {
        // Arrange

        var (manager, _, project) = await InitManager();
        await manager.LoadProjectAsync(project.ProjectFile);
        const string expectedFile = "main.asm";
        manager.AddDeviceToProject(expectedFile);

        // Act

        manager.AddDeviceToProject(expectedFile);

        // Assert

        Assert.That(manager.Project.Devices,
            Has.Exactly(1).Matches<string>(s => s == PathHelper.GetFullPath(expectedFile)));
    }

    [Test]
    public async Task RemoveDeviceTest()
    {
        // Arrange

        var (manager, _, project) = await InitManager();
        await manager.LoadProjectAsync(project.ProjectFile);
        const string expectedFile = "main.asm";
        manager.AddDeviceToProject(expectedFile);
        var propertyChangedAssert = new PropertyChangedAssert(manager);

        // Act

        manager.RemoveDeviceFromProject(expectedFile);

        // Assert

        Assert.That(manager.Project.Files, Does.Not.Contain(expectedFile));
        propertyChangedAssert.Assert(nameof(project.Devices));
    }

    [Test]
    public async Task RemoveUnExistingDeviceTest()
    {
        // Arrange

        var (manager, _, project) = await InitManager();
        await manager.LoadProjectAsync(project.ProjectFile);
        const string expectedFile = "main.asm";

        // Act

        manager.RemoveDeviceFromProject(expectedFile);

        // Assert

        Assert.That(manager.Project.Files, Does.Not.Contain(expectedFile));
    }

    private static async Task<Project> InitProject(string name)
    {
        var directoryPath = Directory.GetCurrentDirectory();
        var expectedPath =
            $"{directoryPath}{Path.DirectorySeparatorChar}{name}.{ProjectManager.ProjectExtension}";

        var project = new Project
        {
            ProjectFile = expectedPath
        };

        await project.ToJsonAsync();

        return project;
    }

    private static async Task<(IProjectManager, IProjectProvider, IProject)> InitManager(
        [CallerMemberName] string name = null)
    {
        var project = await InitProject(name);
        var provider = new ProjectProvider();
        var manager = new ProjectManager(provider);
        return (manager, provider, project);
    }

    private static async Task<(IProjectManager, Mock<IProjectProvider>, IProject)> InitMockManager(
        [CallerMemberName] string name = null)
    {
        var project = await InitProject(name);
        
        var projectProvider = new Mock<IProjectProvider>();
        projectProvider
            .Setup(m => m.OpenProjectAsync(It.IsAny<string>()))
            .ReturnsAsync(project);

        var manager = new ProjectManager(projectProvider.Object);

        return (manager, projectProvider, project);
    }
}