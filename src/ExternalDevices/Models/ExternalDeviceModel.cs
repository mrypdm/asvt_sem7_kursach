using System;
using System.Collections.Generic;
using ExternalDeviceSdk;

namespace ExternalDevices.Models;

/// <inheritdoc />
public sealed class ExternalDeviceModel : IExternalDeviceModel
{
    private bool _disposed;

    /// <inheritdoc />
    public string AssemblyPath { get; set; }

    /// <inheritdoc />
    public IAssemblyContext AssemblyContext { get; set; }

    /// <inheritdoc />
    public List<IExternalDevice> ExternalDevices { get; set; }

    private void Unload()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        ExternalDevices.ForEach(d => d.Dispose());
        AssemblyContext.Dispose();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Unload();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    ~ExternalDeviceModel()
    {
        Unload();
    }
}