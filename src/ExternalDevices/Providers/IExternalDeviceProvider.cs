using ExternalDevices.Models;

namespace ExternalDevices.Providers;

/// <summary>
/// Provider for <see cref="ExternalDeviceModel"/>
/// </summary>
public interface IExternalDeviceProvider
{
    /// <summary>
    /// Loads device from assembly
    /// </summary>
    /// <param name="assemblyFilePath">Path to assembly file</param>
    /// <returns>Device model</returns>
    IExternalDeviceModel LoadDevice(string assemblyFilePath);
}