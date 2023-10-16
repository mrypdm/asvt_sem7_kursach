using System;
using ExternalDevices.Managers;
using ExternalDevices.Models;
using ExternalDevices.Providers;
using Moq;

namespace ExternalDevicesTests;

public class ExternalDevicesManagerTests
{
    [Test]
    public void ValidateCorrectDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager(new ExternalDeviceProvider());

        // Act & Assert
        Assert.That(manager.ValidateDevice(Constants.DefaultExternalDevice), Is.True);
    }

    [Test]
    public void ValidateIncorrectDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager(new ExternalDeviceProvider());

        // Act & Assert
        Assert.That(manager.ValidateDevice(Constants.InvalidExternalDevice), Is.False);
    }

    [Test]
    public void LoadDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager(new ExternalDeviceProvider());

        // Act
        manager.AddDevice(Constants.DefaultExternalDevice);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(1));
    }

    [Test]
    public void DeleteDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager(new ExternalDeviceProvider());
        manager.AddDevice(Constants.DefaultExternalDevice);

        // Act
        manager.RemoveDevice(Constants.DefaultExternalDevice);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(0));
    }

    [Test]
    public void DeleteUnexistingDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager(new ExternalDeviceProvider());

        // Act
        manager.RemoveDevice(Constants.DefaultExternalDevice);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(0));
    }

    [Test]
    public void AddDuplicateDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager(new ExternalDeviceProvider());
        manager.AddDevice(Constants.DefaultExternalDevice);

        // Act
        manager.AddDevice(Constants.DefaultExternalDevice);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(1));
    }

    [Test]
    public void AddDoubleDeviceDevice()
    {
        // Arrange
        var manager = new ExternalDevicesManager(new ExternalDeviceProvider());

        // Act
        manager.AddDevice(Constants.DoubleExternalDevice);

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(2));
    }

    [Test]
    public void ProviderCalledAtAdd_DisposeCalledAtDelete()
    {
        // Arrange

        var model = new Mock<IExternalDeviceModel>();
        model.Setup(m => m.AssemblyPath).Returns(Constants.DefaultExternalDevice);

        var provider = new Mock<IExternalDeviceProvider>();
        provider.Setup(p => p.LoadDevice(It.IsAny<string>())).Returns(model.Object);

        var manager = new ExternalDevicesManager(provider.Object);
        manager.AddDevice(Constants.DefaultExternalDevice);

        // Act

        manager.RemoveDevice(Constants.DefaultExternalDevice);

        // Assert

        model.Verify(m => m.Dispose(), Times.Once);
        provider.Verify(p => p.LoadDevice(Constants.DefaultExternalDevice), Times.Once);
    }

    [Test]
    public void ClearDevices()
    {
        // Arrange
        var manager = new ExternalDevicesManager(new ExternalDeviceProvider());

        manager.AddDevice(Constants.DefaultExternalDevice);
        manager.AddDevice(Constants.DoubleExternalDevice);

        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(3));

        // Act
        manager.Clear();

        // Assert
        Assert.That(manager.ExternalDevices, Has.Count.EqualTo(0));
    }

    [Test]
    public void ThrowIfDisposed()
    {
        // Arrange
        var manager = new ExternalDevicesManager(new ExternalDeviceProvider());
        manager.Dispose();
        
        // Act & Assert

        Assert.Throws<ObjectDisposedException>(() => manager.AddDevice(Constants.DefaultExternalDevice));
        Assert.Throws<ObjectDisposedException>(() => manager.RemoveDevice(Constants.DefaultExternalDevice));
        Assert.Throws<ObjectDisposedException>(() => manager.Clear());
    }
}