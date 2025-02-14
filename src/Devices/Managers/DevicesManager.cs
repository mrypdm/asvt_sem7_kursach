﻿using System;
using System.Collections.Generic;
using System.Linq;
using Devices.Contexts;
using Devices.Providers;
using DeviceSdk;

namespace Devices.Managers;

/// <inheritdoc />
public sealed class DevicesManager : IDevicesManager
{
    private List<IDeviceContext> _contexts = new();

    private readonly IDeviceProvider _provider;

    private List<IDeviceContext> SafeContexts => _contexts ?? throw new ObjectDisposedException("Manager is disposed");

    public DevicesManager(IDeviceProvider provider)
    {
        _provider = provider;
    }

    /// <summary>
    /// Collection of connected  devices
    /// </summary>
    public IReadOnlyCollection<IDevice> Devices => SafeContexts.SelectMany(d => d.Devices).ToList();

    /// <inheritdoc />
    public void Add(string devicePath)
    {
        if (SafeContexts.SingleOrDefault(d => d.AssemblyPath == devicePath) != null)
        {
            return;
        }

        var device = _provider.Load(devicePath);

        SafeContexts.Add(device);
    }

    /// <inheritdoc />
    public void Remove(string devicePath)
    {
        var model = SafeContexts.SingleOrDefault(d => d.AssemblyPath == devicePath);

        if (model == null)
        {
            return;
        }

        model.Dispose();
        SafeContexts.Remove(model);
    }

    /// <inheritdoc />
    public void Clear()
    {
        SafeContexts.ForEach(d => d.Dispose());
        SafeContexts.Clear();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_contexts == null)
        {
            return;
        }

        Clear();
        _contexts = null;
    }
}