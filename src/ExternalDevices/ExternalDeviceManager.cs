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

    public IReadOnlyCollection<IExternalDevice> ExternalDevices => _devices.Select(d => d.ExternalDevice).ToList();

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

        var (context, device) = LoadDeviceFromAssembly(devicePath);

        _devices.Add(new ExternalDeviceModel
        {
            AssemblyPath = devicePath,
            AssemblyContext = context,
            ExternalDevice = device
        });
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
            LoadDeviceFromAssembly(devicePath);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static (AssemblyLoadContext, IExternalDevice) LoadDeviceFromAssembly(string assemblyFilePath)
    {
        var context = new AssemblyLoadContext(assemblyFilePath, true);
        var assembly = context.LoadFromAssemblyPath(assemblyFilePath);

        var type = assembly
                       .GetExportedTypes()
                       .FirstOrDefault(t =>
                           t.IsClass && t.GetInterfaces().Any(i => i.FullName == typeof(IExternalDevice).FullName))
                   ?? throw new InvalidOperationException("Cannot find external device class");

        var device = Activator.CreateInstance(type) as IExternalDevice ??
                     throw new InvalidOperationException("Cannot create instance of device");

        return (context, device);
    }
}