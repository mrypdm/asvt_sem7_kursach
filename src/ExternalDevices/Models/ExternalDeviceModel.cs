using System.Collections.Generic;
using ExternalDeviceSdk;

namespace ExternalDevices.Models;

/// <inheritdoc />
public sealed class ExternalDeviceModel : IExternalDeviceModel
{
    private bool _disposed;

    /// <inheritdoc />
    public string AssemblyPath { get; init; }

    /// <inheritdoc />
    public IAssemblyContext AssemblyContext { get; init; }

    /// <inheritdoc />
    public List<IExternalDevice> ExternalDevices { get; init; }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        ExternalDevices.ForEach(d => d.Dispose());
        AssemblyContext.Dispose();
    }
}