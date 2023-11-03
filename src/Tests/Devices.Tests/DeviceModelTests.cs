using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Devices.Contexts;
using DeviceSdk;

namespace Devices.Tests;

public class DeviceModelTests
{
    [Test]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    public void ThrowIfDisposed()
    {
        // Arrange
        var model = new DeviceContext(new AssemblyContext(null), Array.Empty<IDevice>());
        model.Dispose();

        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => model.Devices.ToList());
        Assert.Throws<ObjectDisposedException>(() => model.AssemblyPath.ToList());
    }
}