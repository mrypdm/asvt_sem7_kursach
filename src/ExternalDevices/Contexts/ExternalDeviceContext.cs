using System;
using System.Collections.Generic;
using System.Linq;
using ExternalDeviceSdk;

namespace ExternalDevices.Contexts;

/// <inheritdoc />
public sealed class ExternalDeviceContext : IExternalDeviceContext
{
    private AssemblyContext _context;

    private List<IExternalDevice> _devices;

    public ExternalDeviceContext(AssemblyContext context, IEnumerable<IExternalDevice> devices)
    {
        _context = context;
        _devices = devices.ToList();
    }

    /// <inheritdoc />
    public string AssemblyPath =>
        _context?.Assembly.Location ?? throw new ObjectDisposedException("External device is disposed");

    /// <inheritdoc />
    public IReadOnlyCollection<IExternalDevice> ExternalDevices =>
        _devices ?? throw new ObjectDisposedException("External device is disposed");

    /// <inheritdoc />
    public void Dispose()
    {
        _devices?.ForEach(d => d.Dispose());
        _context?.Dispose();

        _devices = null;
        _context = null;
    }
}