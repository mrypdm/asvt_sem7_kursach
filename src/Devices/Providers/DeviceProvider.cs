using System;
using System.Linq;
using Devices.Contexts;
using DeviceSdk;

namespace Devices.Providers;

/// <inheritdoc />
public class DeviceProvider : IDeviceProvider
{
    private static TType CreateInstance<TType>(Type type, out Exception error) where TType : class
    {
        try
        {
            var res = Activator.CreateInstance(type) as TType;
            error = null;
            return res;
        }
        catch (Exception e)
        {
            error = e;
            return null;
        }
    }

    /// <inheritdoc />
    public IDeviceContext Load(string assemblyFilePath)
    {
        var context = new AssemblyContext(assemblyFilePath);
        var assembly = context.Load(assemblyFilePath);

        var types = assembly
            .GetExportedTypes()
            .Where(t =>
                t.IsClass && t.GetInterfaces().Any(i => i.FullName == typeof(IDevice).FullName))
            .ToList();

        if (!types.Any())
        {
            throw new InvalidOperationException("Cannot find devices");
        }

        var devices = types
            .Select(
                t => CreateInstance<IDevice>(t, out var err)
                     ?? throw new InvalidOperationException($"Cannot create instance of device '{t.FullName}'", err));

        return new DeviceContext(context, devices);
    }
    
    /// <inheritdoc />
    public bool TryLoad(string assemblyFilePath, out IDeviceContext device)
    {
        try
        {
            device = Load(assemblyFilePath);
            return true;
        }
        catch
        {
            device = null;
            return false;
        }
    }
}