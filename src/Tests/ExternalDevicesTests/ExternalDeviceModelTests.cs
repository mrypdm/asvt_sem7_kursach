using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ExternalDevices.Contexts;
using ExternalDeviceSdk;

namespace ExternalDevicesTests;

public class ExternalDeviceModelTests
{
    [Test]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    public void ThrowIfDisposed()
    {
        // Arrange
        var model = new ExternalDeviceContext(new AssemblyContext(null), Array.Empty<IExternalDevice>());
        model.Dispose();

        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => model.ExternalDevices.ToList());
        Assert.Throws<ObjectDisposedException>(() => model.AssemblyPath.ToList());
    }
}