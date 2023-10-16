using System;
using System.Linq;
using ExternalDevices.Models;
using ExternalDeviceSdk;

namespace ExternalDevices.Providers;

/// <inheritdoc />
public class ExternalDeviceProvider : IExternalDeviceProvider
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
    public IExternalDeviceContext LoadDevice(string assemblyFilePath)
    {
        var context = new AssemblyContext(assemblyFilePath);
        var assembly = context.Load(assemblyFilePath);

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
            .Select(
                t => CreateInstance<IExternalDevice>(t, out var err)
                     ?? throw new InvalidOperationException($"Cannot create instance of device '{t.FullName}'", err));

        return new ExternalDeviceContext(context, devices);
    }
}