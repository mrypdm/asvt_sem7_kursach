using Devices.Providers;

namespace Devices.Validators;

/// <inheritdoc />
public class DeviceValidator : IDeviceValidator
{
    private readonly IDeviceProvider _provider;

    public DeviceValidator(IDeviceProvider provider)
    {
        _provider = provider;
    }
    
    /// <inheritdoc />
    public bool Validate(string path)
    {
        var res = _provider.TryLoad(path, out var device);
        device?.Dispose();
        return res;
    }
}