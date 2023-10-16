using System.IO;
using ExternalDevices;

namespace ExternalDevicesTests;

public class ExternalDevicesManagerTests
{
    private static string DefaultExternalDevice =>
        Path.Combine(Directory.GetCurrentDirectory(), "DemoExternalDevice/DemoExternalDevice.dll");

    private static string DoubleExternalDevice =>
        Path.Combine(Directory.GetCurrentDirectory(), "DemoExternalDevice/DoubleExternalDevice.dll");

    private static string InvalidExternalDevice =>
        Path.Combine(Directory.GetCurrentDirectory(), "DemoExternalDevice/InvalidExternalDevice.dll");

    [Test]
    public void ValidateCorrectDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();

        // Act & Assert
        Assert.That(manager.ValidateDevice(DefaultExternalDevice), Is.True);
    }

    [Test]
    public void ValidateIncorrectDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();

        // Act & Assert
        Assert.That(manager.ValidateDevice(InvalidExternalDevice), Is.False);
    }

    [Test]
    public void LoadDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();

        // Act
        manager.AddDevice(DefaultExternalDevice);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(1));
    }

    [Test]
    public void DeleteDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();
        manager.AddDevice(DefaultExternalDevice);

        // Act
        manager.RemoveDevice(DefaultExternalDevice);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(0));
    }

    [Test]
    public void DeleteUnexistingDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();

        // Act
        manager.RemoveDevice(DefaultExternalDevice);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(0));
    }

    [Test]
    public void AddDuplicateDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();
        manager.AddDevice(DefaultExternalDevice);

        // Act
        manager.AddDevice(DefaultExternalDevice);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(1));
    }

    [Test]
    public void AddDoubleDeviceDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();

        // Act
        manager.AddDevice(DoubleExternalDevice);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(2));
    }
}