using System;
using Devices.Providers;

namespace DevicesTests;

public class DeviceProviderTests
{
    [Test]
    [TestCaseSource(nameof(LoadDefaultDeviceSource))]
    public void LoadDefaultDevice(string path, int count)
    {
        // Arrange
        var provider = new DeviceProvider();

        // Act
        var model = provider.Load(path);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(model.AssemblyPath, Is.EqualTo(path));
            Assert.That(model.Devices, Has.Count.EqualTo(count));
        });
    }

    [Test]
    public void LoadInvalidDevice()
    {
        // Arrange
        var provider = new DeviceProvider();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => provider.Load(Constants.InvalidDevice));
    }

    [Test]
    public void TryLoadInvalidDevice()
    {
        // Arrange
        var provider = new DeviceProvider();

        // Act
        var res = provider.TryLoad(Constants.InvalidDevice, out var device);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(res, Is.False);
            Assert.That(device, Is.Null);
        });
    }

    // ReSharper disable once InconsistentNaming
    private static object[] LoadDefaultDeviceSource =
    {
        new object[] { Constants.DefaultDevice, 1 },
        new object[] { Constants.DoubleDevice, 2 }
    };
}