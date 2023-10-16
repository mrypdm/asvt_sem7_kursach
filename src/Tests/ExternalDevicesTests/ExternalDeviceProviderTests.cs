using System.Collections.Generic;
using ExternalDevices.Models;
using ExternalDevices.Providers;
using ExternalDeviceSdk;
using Moq;

namespace ExternalDevicesTests;

public class ExternalDeviceProviderTests
{
    [Test]
    public void DisposeForDeviceIsCalled()
    {
        // Arrange

        var device = new Mock<IExternalDevice>();
        var context = new Mock<IAssemblyContext>();

        var model = new ExternalDeviceModel
        {
            AssemblyPath = string.Empty,
            AssemblyContext = context.Object,
            ExternalDevices = new List<IExternalDevice> { device.Object, device.Object }
        };

        var provider = new ExternalDeviceProvider();

        // Act

        provider.UnloadDevice(model);

        // Assert

        device.Verify(d => d.Dispose(), Times.Exactly(2));
        context.Verify(d => d.Unload(), Times.Once);
    }
}