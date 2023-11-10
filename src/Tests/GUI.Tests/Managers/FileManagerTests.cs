using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using GUI.Managers;
using GUI.Models;
using Moq;
using Shared.Helpers;

namespace GUI.Tests.Managers;

public class FileManagerTests
{
    [Test]
    public async Task GetFileOpenTest()
    {
        // Arrange

        const string filePath = "C:\\c.txt";
        var file = new Mock<IStorageFile>();
        file.Setup(m => m.Path).Returns(new Uri(filePath));

        var storageProvider = new Mock<IStorageProvider>();
        storageProvider
            .Setup(m => m.OpenFilePickerAsync(It.IsAny<FilePickerOpenOptions>()))
            .ReturnsAsync(new[] { file.Object });

        var options = new FilePickerOpenOptions { AllowMultiple = false };

        var fileManager = new FileManager();

        // Act

        var selectedFile = await fileManager.GetFileAsync(storageProvider.Object, options);

        // Assert

        Assert.That(selectedFile, Is.EqualTo(filePath));
        storageProvider.Verify(m => m.OpenFilePickerAsync(options), Times.Once);
        file.Verify(m => m.Path, Times.Once);
    }

    [Test]
    public async Task GetFileSaveTest()
    {
        // Arrange

        const string filePath = "C:\\c.txt";
        var file = new Mock<IStorageFile>();
        file.Setup(m => m.Path).Returns(new Uri(filePath));

        var storageProvider = new Mock<IStorageProvider>();
        storageProvider
            .Setup(m => m.SaveFilePickerAsync(It.IsAny<FilePickerSaveOptions>()))
            .ReturnsAsync(file.Object);

        var options = new FilePickerSaveOptions();

        var fileManager = new FileManager();

        // Act

        var selectedFile = await fileManager.GetFileAsync(storageProvider.Object, options);

        // Assert

        Assert.That(selectedFile, Is.EqualTo(filePath));
        storageProvider.Verify(m => m.SaveFilePickerAsync(options), Times.Once);
        file.Verify(m => m.Path, Times.Once);
    }

    [Test]
    [TestCaseSource(nameof(GetFileWithInvalidOptionsTestSource))]
    public void GetFileWithInvalidOptionsTest(PickerOptions options)
    {
        // Arrange

        var storageProvider = new Mock<IStorageProvider>();

        var manager = new FileManager();

        // Act & Assert

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await manager.GetFileAsync(storageProvider.Object, options);
        });
    }

    [Test]
    public async Task GetFileOpenAbortTest()
    {
        // Arrange

        var storageProvider = new Mock<IStorageProvider>();
        storageProvider
            .Setup(m => m.OpenFilePickerAsync(It.IsAny<FilePickerOpenOptions>()))
            .ReturnsAsync(Array.Empty<IStorageFile>());

        var options = new FilePickerOpenOptions { AllowMultiple = false };

        var fileManager = new FileManager();

        // Act

        var filePath = await fileManager.GetFileAsync(storageProvider.Object, options);

        // Assert

        Assert.That(filePath, Is.Null);
        storageProvider.Verify(m => m.OpenFilePickerAsync(options), Times.Once);
    }

    [Test]
    public async Task GetFileSaveAbortTest()
    {
        // Arrange

        var storageProvider = new Mock<IStorageProvider>();
        storageProvider
            .Setup(m => m.SaveFilePickerAsync(It.IsAny<FilePickerSaveOptions>()))
            .ReturnsAsync(null as IStorageFile);

        var options = new FilePickerSaveOptions();

        var fileManager = new FileManager();

        // Act

        var filePath = await fileManager.GetFileAsync(storageProvider.Object, options);

        // Assert

        Assert.That(filePath, Is.Null);
        storageProvider.Verify(m => m.SaveFilePickerAsync(options), Times.Once);
    }

    [Test]
    public async Task CreateFileTest()
    {
        // Arrange

        var fileName = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{nameof(CreateFileTest)}.txt";
        var file = new Mock<IStorageFile>();
        file.Setup(m => m.Path).Returns(new Uri(fileName));

        FilePickerSaveOptions options = null;
        var storageProvider = new Mock<IStorageProvider>();
        storageProvider
            .Setup(m => m.SaveFilePickerAsync(It.IsAny<FilePickerSaveOptions>()))
            .Callback<FilePickerSaveOptions>(o => options = o)
            .ReturnsAsync(file.Object);

        var manager = new FileManager();

        // Act

        var selectedFile = await manager.CreateFile(storageProvider.Object, null, fileName);

        // Assert

        storageProvider.Verify(m => m.SaveFilePickerAsync(options), Times.Once);
        file.Verify(m => m.Path, Times.Once);
        Assert.Multiple(() =>
        {
            Assert.That(options.SuggestedFileName, Is.EqualTo(fileName));
            Assert.That(selectedFile.FilePath, Is.EqualTo(fileName));
            Assert.That(selectedFile.Text, Is.Empty);
            Assert.That(PathHelper.GetPathType(fileName), Is.EqualTo(PathHelper.PathType.File));
        });
    }

    [Test]
    public async Task CreateFileAbortTest()
    {
        // Arrange

        var storageProvider = new Mock<IStorageProvider>();
        storageProvider
            .Setup(m => m.SaveFilePickerAsync(It.IsAny<FilePickerSaveOptions>()))
            .ReturnsAsync(null as IStorageFile);

        var manager = new FileManager();

        // Act

        var file = await manager.CreateFile(storageProvider.Object, null, null);

        // Assert

        Assert.That(file, Is.Null);
    }

    [Test]
    public async Task OpenFilesTest()
    {
        // Arrange

        var dir = Directory.GetCurrentDirectory();
        var sep = Path.DirectorySeparatorChar;

        var path1 = $"{dir}{sep}{nameof(OpenFilesTest)}1.txt";
        var content1 = "Test1";
        var file1 = new Mock<IStorageFile>();
        file1.Setup(m => m.Path).Returns(new Uri(path1));

        var path2 = $"{dir}{sep}{nameof(OpenFilesTest)}2.txt";
        var content2 = "Test2";
        var file2 = new Mock<IStorageFile>();
        file2.Setup(m => m.Path).Returns(new Uri(path2));

        await File.WriteAllTextAsync(path1, content1);
        await File.WriteAllTextAsync(path2, content2);

        var storageProvider = new Mock<IStorageProvider>();
        storageProvider
            .Setup(m => m.OpenFilePickerAsync(It.IsAny<FilePickerOpenOptions>()))
            .ReturnsAsync(new[] { file1.Object, file2.Object });

        var manager = new FileManager();

        // Act

        var files = await manager.OpenFilesAsync(storageProvider.Object);

        // Assert

        Assert.That(files, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(files.ElementAt(0).FilePath, Is.EqualTo(path1));
            Assert.That(files.ElementAt(1).FilePath, Is.EqualTo(path2));
            Assert.That(files.ElementAt(0).Text, Is.EqualTo(content1));
            Assert.That(files.ElementAt(1).Text, Is.EqualTo(content2));
        });

        storageProvider.Verify(m => m.OpenFilePickerAsync(It.IsAny<FilePickerOpenOptions>()), Times.Once);
    }
    
    [Test]
    public async Task OpenFilesAbortTest()
    {
        // Arrange
        var storageProvider = new Mock<IStorageProvider>();
        storageProvider
            .Setup(m => m.OpenFilePickerAsync(It.IsAny<FilePickerOpenOptions>()))
            .ReturnsAsync(Array.Empty<IStorageFile>());

        var manager = new FileManager();

        // Act

        var files = await manager.OpenFilesAsync(storageProvider.Object);

        // Assert

        Assert.That(files, Is.Empty);
        storageProvider.Verify(m => m.OpenFilePickerAsync(It.IsAny<FilePickerOpenOptions>()), Times.Once);
    }

    [Test]
    public async Task OpenFileTest()
    {
        // Arrange
        
        var file = new FileModel
        {
            FilePath = $"{nameof(OpenFileTest)}.txt",
            Text = "test"
        };

        var manager = new FileManager();
        await manager.WriteFileAsync(file);

        // Act

        var openedFile = await manager.OpenFileAsync(file.FilePath);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(openedFile.FilePath, Is.EqualTo(file.FilePath));
            Assert.That(openedFile.Text, Is.EqualTo(file.Text));
        });
    }

    [Test]
    public void OpenUnExistingFileTest()
    {
        // Arrange
        
        var manager = new FileManager();
        
        // Act & Assert

        Assert.ThrowsAsync<FileNotFoundException>(async () => await manager.OpenFileAsync("NotExist"));
    }

    [Test]
    public async Task WriteTest()
    {
        // Arrange

        var file = new FileModel
        {
            FilePath = $"{nameof(WriteTest)}.txt",
            Text = "test"
        };

        var manager = new FileManager();

        // Act

        await manager.WriteFileAsync(file);

        // Assert

        var content = await File.ReadAllTextAsync(file.FilePath);
        Assert.That(content, Is.EqualTo(file.Text));
    }

    [Test]
    public async Task DeleteTest()
    {
        // Arrange

        var file = new FileModel
        {
            FilePath = $"{nameof(DeleteTest)}.txt",
            Text = "test"
        };

        var manager = new FileManager();
        await manager.WriteFileAsync(file);

        // Act

        await manager.DeleteAsync(file);

        // Assert

        Assert.That(PathHelper.GetPathType(file.FilePath), Is.EqualTo(PathHelper.PathType.UnExisting));
    }

    private static readonly object[] GetFileWithInvalidOptionsTestSource =
    {
        new FilePickerOpenOptions { AllowMultiple = true },
        new FolderPickerOpenOptions()
    };
}