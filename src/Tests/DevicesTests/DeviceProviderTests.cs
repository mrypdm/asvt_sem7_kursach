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
        var model = provider.LoadDevice(path);

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
        Assert.Throws<InvalidOperationException>(() => provider.LoadDevice(Constants.InvalidDevice));
    }

    // ReSharper disable once InconsistentNaming
    private static object[] LoadDefaultDeviceSource =
    {
        new object[] { Constants.DefaultDevice, 1 },
        new object[] { Constants.DoubleDevice, 2 }
    };
}