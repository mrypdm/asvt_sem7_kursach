using System;
using System.Collections.Generic;
using System.Linq;
using ExternalDevices.Models;
using ExternalDevices.Providers;
using ExternalDeviceSdk;

namespace ExternalDevices.Managers;

/// <inheritdoc />
public sealed class ExternalDevicesManager : IExternalDevicesManager
{
    private List<IExternalDeviceContext> _devices = new();

    private readonly IExternalDeviceProvider _provider;

    private List<IExternalDeviceContext> SafeDevices =>
        _devices ?? throw new ObjectDisposedException("Manager is disposed");

    public ExternalDevicesManager(IExternalDeviceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Collection of connected external devices
    /// </summary>
    public IReadOnlyCollection<IExternalDevice> ExternalDevices =>
        SafeDevices.SelectMany(d => d.ExternalDevices).ToList();

    /// <inheritdoc />
    public void AddDevice(string devicePath)
    {
        if (SafeDevices.SingleOrDefault(d => d.AssemblyPath == devicePath) != null)
        {
            return;
        }

        var device = _provider.LoadDevice(devicePath);

        SafeDevices.Add(device);
    }

    /// <inheritdoc />
    public void RemoveDevice(string devicePath)
    {
        var model = SafeDevices.SingleOrDefault(d => d.AssemblyPath == devicePath);

        if (model == null)
        {
            return;
        }

        model.Dispose();
        SafeDevices.Remove(model);
    }

    /// <inheritdoc />
    public bool ValidateDevice(string devicePath)
    {
        try
        {
            var device = _provider.LoadDevice(devicePath);
            device.Dispose();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public void Clear()
    {
        SafeDevices.ForEach(d => d.Dispose());
        SafeDevices.Clear();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_devices == null)
        {
            return;
        }

        Clear();
        _devices = null;
    }
}