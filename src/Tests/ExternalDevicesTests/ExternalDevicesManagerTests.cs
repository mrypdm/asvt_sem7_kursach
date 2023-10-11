using System.IO;
using ExternalDevices;

namespace ExternalDevicesTests;

public class ExternalDevicesManagerTests
{
    private const string DefaultExternalDevice = "DemoExternalDevice/DemoExternalDevice.dll";
    private const string InvalidExternalDevice = "DemoExternalDevice/InvalidExternalDevice.dll";

    private static string DefaultExternalDeviceAbsolutePath =>
        Path.Combine(Directory.GetCurrentDirectory(), DefaultExternalDevice);

    private static string InvalidExternalDeviceAbsolutePath =>
        Path.Combine(Directory.GetCurrentDirectory(), InvalidExternalDevice);

    [Test]
    public void ValidateCorrectDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();

        // Act & Assert
        Assert.That(manager.ValidateDevice(DefaultExternalDeviceAbsolutePath), Is.True);
    }

    [Test]
    public void ValidateIncorrectDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();

        // Act & Assert
        Assert.That(manager.ValidateDevice(InvalidExternalDeviceAbsolutePath), Is.False);
    }

    [Test]
    public void LoadDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();

        // Act
        manager.AddDevice(DefaultExternalDeviceAbsolutePath);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(1));
    }

    [Test]
    public void DeleteDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();
        manager.AddDevice(DefaultExternalDeviceAbsolutePath);

        // Act
        manager.RemoveDevice(DefaultExternalDeviceAbsolutePath);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(0));
    }

    [Test]
    public void DeleteUnexistingDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();

        // Act
        manager.RemoveDevice(DefaultExternalDeviceAbsolutePath);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(0));
    }

    [Test]
    public void AddDuplicateDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager();
        manager.AddDevice(DefaultExternalDeviceAbsolutePath);

        // Act
        manager.AddDevice(DefaultExternalDeviceAbsolutePath);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(1));
    }
}