using System;
using Devices.Contexts;
using Devices.Managers;
using Devices.Providers;
using Moq;

namespace DevicesTests;

public class DevicesManagerTests
{
    [Test]
    public void LoadDevice()
    {
        // Arrange
        var manager = new DevicesManager(new DeviceProvider());

        // Act
        manager.Add(Constants.DefaultDevice);

        // Assert
        Assert.That(manager.Devices, Has.Count.EqualTo(1));
    }

    [Test]
    public void DeleteDevice()
    {
        // Arrange
        var manager = new DevicesManager(new DeviceProvider());
        manager.Add(Constants.DefaultDevice);

        // Act
        manager.Remove(Constants.DefaultDevice);

        // Assert
        Assert.That(manager.Devices, Has.Count.EqualTo(0));
    }

    [Test]
    public void DeleteUnexistingDevice()
    {
        // Arrange
        var manager = new DevicesManager(new DeviceProvider());

        // Act
        manager.Remove(Constants.DefaultDevice);

        // Assert
        Assert.That(manager.Devices, Has.Count.EqualTo(0));
    }

    [Test]
    public void AddDuplicateDevice()
    {
        // Arrange
        var manager = new DevicesManager(new DeviceProvider());
        manager.Add(Constants.DefaultDevice);

        // Act
        manager.Add(Constants.DefaultDevice);

        // Assert
        Assert.That(manager.Devices, Has.Count.EqualTo(1));
    }

    [Test]
    public void AddDoubleDeviceDevice()
    {
        // Arrange
        var manager = new DevicesManager(new DeviceProvider());

        // Act
        manager.Add(Constants.DoubleDevice);

        // Assert
        Assert.That(manager.Devices, Has.Count.EqualTo(2));
    }

    [Test]
    public void ProviderCalledAtAdd_DisposeCalledAtDelete()
    {
        // Arrange
        var model = new Mock<IDeviceContext>();
        model.Setup(m => m.AssemblyPath).Returns(Constants.DefaultDevice);

        var provider = new Mock<IDeviceProvider>();
        provider.Setup(p => p.Load(It.IsAny<string>())).Returns(model.Object);

        var manager = new DevicesManager(provider.Object);
        manager.Add(Constants.DefaultDevice);

        // Act
        manager.Remove(Constants.DefaultDevice);

        // Assert
        model.Verify(m => m.Dispose(), Times.Once);
        provider.Verify(p => p.Load(Constants.DefaultDevice), Times.Once);
    }

    [Test]
    public void ClearDevices()
    {
        // Arrange
        var manager = new DevicesManager(new DeviceProvider());

        manager.Add(Constants.DefaultDevice);
        manager.Add(Constants.DoubleDevice);

        Assert.That(manager.Devices, Has.Count.EqualTo(3));

        // Act
        manager.Clear();

        // Assert
        Assert.That(manager.Devices, Has.Count.EqualTo(0));
    }

    [Test]
    public void ThrowIfDisposed()
    {
        // Arrange
        var manager = new DevicesManager(new DeviceProvider());
        manager.Dispose();
        
        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => manager.Add(Constants.DefaultDevice));
        Assert.Throws<ObjectDisposedException>(() => manager.Remove(Constants.DefaultDevice));
        Assert.Throws<ObjectDisposedException>(() => manager.Clear());
    }
}