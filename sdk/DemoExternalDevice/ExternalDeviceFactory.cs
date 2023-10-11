using ExternalDeviceSdk;

namespace DemoExternalDevice;

/// <summary>
/// External device factory
/// </summary>
public class ExternalDeviceFactory
{
    /// <summary>
    /// Creates external device
    /// </summary>
    public IExternalDevice GetDevice() => new ExternalDevice();
}