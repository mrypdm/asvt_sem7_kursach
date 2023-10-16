using System;
using ExternalDevices.Providers;

namespace ExternalDevicesTests;

[NonParallelizable]
public class ExternalDeviceProviderTests
{
    [Test]
    [TestCaseSource(nameof(LoadDefaultDeviceSource))]
    public void LoadDefaultDevice(string path, int count)
    {
        // Arrange

        var provider = new ExternalDeviceProvider();

        // Act

        var model = provider.LoadDevice(path);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(model.AssemblyPath, Is.EqualTo(path));
            Assert.That(model.ExternalDevices, Has.Count.EqualTo(count));
            Assert.That(model.AssemblyContext.Assembly.Location, Is.EqualTo(path));
        });
    }

    [Test]
    public void LoadInvalidDevice()
    {
        // Arrange

        var provider = new ExternalDeviceProvider();

        // Act & Assert

        Assert.Throws<InvalidOperationException>(() => provider.LoadDevice(Constants.InvalidExternalDevice));
    }

    // ReSharper disable once InconsistentNaming
    private static object[] LoadDefaultDeviceSource =
    {
        new object[] { Constants.DefaultExternalDevice, 1 },
        new object[] { Constants.DoubleExternalDevice, 2 }
    };
}