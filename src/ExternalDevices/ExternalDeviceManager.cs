using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using ExternalDeviceSdk;

namespace ExternalDevices;

/// <summary>
/// Manager for external devices
/// </summary>
public class ExternalDevicesManager
{
    private readonly List<ExternalDeviceModel> _devices = new();

    /// <summary>
    /// Collection of connected external devices
    /// </summary>
    public IReadOnlyCollection<IExternalDevice> ExternalDevices
        => _devices.SelectMany(d => d.ExternalDevices).ToList();

    /// <summary>
    /// Adds new device
    /// </summary>
    /// <param name="devicePath">Path to device</param>
    public void AddDevice(string devicePath)
    {
        if (_devices.SingleOrDefault(d => d.AssemblyPath == devicePath) != null)
        {
            return;
        }

        var device = LoadDeviceFromAssembly(devicePath);

        _devices.Add(device);
    }

    /// <summary>
    /// Removes device
    /// </summary>
    /// <param name="devicePath">Path to external device</param>
    public void RemoveDevice(string devicePath)
    {
        var model = _devices.SingleOrDefault(d => d.AssemblyPath == devicePath);

        if (model == null)
        {
            return;
        }

        model.AssemblyContext.Unload();
        _devices.Remove(model);
    }

    /// <summary>
    /// Validates device
    /// </summary>
    /// <param name="devicePath">Path to external device</param>
    /// <returns>True if device is valid</returns>
    public bool ValidateDevice(string devicePath)
    {
        try
        {
            var device = LoadDeviceFromAssembly(devicePath);
            device.AssemblyContext.Unload();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static ExternalDeviceModel LoadDeviceFromAssembly(string assemblyFilePath)
    {
        var context = new AssemblyLoadContext(assemblyFilePath, true);
        var assembly = context.LoadFromAssemblyPath(assemblyFilePath);

        var types = assembly
            .GetExportedTypes()
            .Where(t =>
                t.IsClass && t.GetInterfaces().Any(i => i.FullName == typeof(IExternalDevice).FullName))
            .ToList();

        if (!types.Any())
        {
            throw new InvalidOperationException("Cannot find external devices.");
        }

        var devices = types
            .Select(t => Activator.CreateInstance(t) as IExternalDevice
                         ?? throw new InvalidOperationException($"Cannot create instance of device '{t.FullName}'"))
            .ToList();

        return new ExternalDeviceModel
        {
            AssemblyPath = assemblyFilePath,
            AssemblyContext = context,
            ExternalDevices = devices
        };
    }
}