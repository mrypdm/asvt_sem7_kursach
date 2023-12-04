using System;
using System.Collections.Generic;
using System.Linq;
using DeviceSdk;

namespace Devices.Contexts;

/// <inheritdoc />
public sealed class DeviceContext : IDeviceContext
{
    private AssemblyContext _context;

    private List<IDevice> _devices;

    public DeviceContext(AssemblyContext context, IEnumerable<IDevice> devices)
    {
        _context = context;
        _devices = devices.ToList();
    }

    /// <inheritdoc />
    public string AssemblyPath =>
        _context?.Assembly.Location ?? throw new ObjectDisposedException("Device is disposed");

    /// <inheritdoc />
    public IReadOnlyCollection<IDevice> Devices =>
        _devices ?? throw new ObjectDisposedException("Device is disposed");

    /// <inheritdoc />
    public void Dispose()
    {
        _devices?.ForEach(d => d.Dispose());
        _context?.Dispose();

        _devices = null;
        _context = null;
    }
}