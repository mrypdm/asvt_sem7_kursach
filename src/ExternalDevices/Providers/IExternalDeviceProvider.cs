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
    ExternalDeviceModel LoadDevice(string assemblyFilePath);
    
    /// <summary>
    /// Unloads device
    /// </summary>
    /// <param name="model">Device model</param>
    void UnloadDevice(ExternalDeviceModel model);
}