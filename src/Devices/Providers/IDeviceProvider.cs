using Devices.Contexts;

namespace Devices.Providers;

/// <summary>
/// Provider for <see cref="DeviceContext"/>
/// </summary>
public interface IDeviceProvider
{
    /// <summary>
    /// Loads device from assembly
    /// </summary>
    /// <param name="assemblyFilePath">Path to assembly file</param>
    /// <returns>Device model</returns>
    IDeviceContext Load(string assemblyFilePath);

    /// <summary>
    /// Tries to load device from assembly
    /// </summary>
    /// <param name="assemblyFilePath">Path to assembly path</param>
    /// <param name="device">Device model</param>
    /// <returns>True if device was loaded, False otherwise</returns>
    bool TryLoad(string assemblyFilePath, out IDeviceContext device);
}