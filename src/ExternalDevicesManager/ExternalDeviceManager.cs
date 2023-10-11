using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using ExternalDeviceSdk;

namespace ExternalDevicesManager;

/// <summary>
/// Manager for external devices
/// </summary>
public class ExternalDevicesManager
{
    private const string ExternalDeviceFactoryClassName = "ExternalDeviceFactory";

    private readonly List<ExternalDeviceModel> _devices = new();

    public ExternalDevicesManager()
    {
    }

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
        
        var(context, device) = LoadDeviceFromAssembly(devicePath);
        
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
        var context = new AssemblyLoadContext(assemblyFilePath);
        var assembly = context.LoadFromAssemblyPath(assemblyFilePath);

        var type = assembly.GetType(ExternalDeviceFactoryClassName) ??
                   throw new InvalidOperationException("Cannot find external device factory");

        dynamic factory = Activator.CreateInstance(type) ??
                          throw new InvalidOperationException("Cannot create factory");

        return (context, factory.GetDevice() as IExternalDevice);
    }
}