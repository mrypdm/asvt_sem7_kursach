using System;
using System.Collections.Generic;
using System.Linq;
using ExternalDevices.Models;
using ExternalDevices.Providers;
using ExternalDeviceSdk;

namespace ExternalDevices.Managers;

/// <inheritdoc />
public class ExternalDevicesManager : IExternalDevicesManager
{
    private readonly List<ExternalDeviceModel> _devices = new();

    private readonly IExternalDeviceProvider _provider;

    public ExternalDevicesManager(IExternalDeviceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Collection of connected external devices
    /// </summary>
    public IReadOnlyCollection<IExternalDevice> ExternalDevices
        => _devices.SelectMany(d => d.ExternalDevices).ToList();

    /// <inheritdoc />
    public void AddDevice(string devicePath)
    {
        if (_devices.SingleOrDefault(d => d.AssemblyPath == devicePath) != null)
        {
            return;
        }

        var device = _provider.LoadDevice(devicePath);

        _devices.Add(device);
    }

    /// <inheritdoc />
    public void RemoveDevice(string devicePath)
    {
        var model = _devices.SingleOrDefault(d => d.AssemblyPath == devicePath);

        if (model == null)
        {
            return;
        }

        _provider.UnloadDevice(model);
        _devices.Remove(model);
    }

    /// <inheritdoc />
    public bool ValidateDevice(string devicePath)
    {
        try
        {
            var device = _provider.LoadDevice(devicePath);
            _provider.UnloadDevice(device);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}