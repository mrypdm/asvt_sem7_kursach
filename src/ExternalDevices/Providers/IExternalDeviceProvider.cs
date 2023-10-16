using ExternalDevices.Contexts;

namespace ExternalDevices.Providers;

/// <summary>
/// Provider for <see cref="ExternalDeviceContext"/>
/// </summary>
public interface IExternalDeviceProvider
{
    /// <summary>
    /// Loads device from assembly
    /// </summary>
    /// <param name="assemblyFilePath">Path to assembly file</param>
    /// <returns>Device model</returns>
    IExternalDeviceContext LoadDevice(string assemblyFilePath);
}