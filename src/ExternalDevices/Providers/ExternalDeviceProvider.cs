using System;
using System.Linq;
using System.Runtime.Loader;
using ExternalDevices.Models;
using ExternalDeviceSdk;

namespace ExternalDevices.Providers;

/// <inheritdoc />
public class ExternalDeviceProvider : IExternalDeviceProvider
{
    /// <inheritdoc />
    public ExternalDeviceModel LoadDevice(string assemblyFilePath)
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

    /// <inheritdoc />
    public void UnloadDevice(ExternalDeviceModel model)
    {
        model.AssemblyContext.Unload();
    }
}