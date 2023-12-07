using System;
using Devices.Providers;
using Shared.Exceptions;

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
    public bool Validate(string path, out string errorMessage)
    {
        try
        {
            _provider.Load(path);
            errorMessage = null;
            return true;
        }
        catch (Exception e)
        {
            errorMessage = e.Message;
            return false;
        }
    }

    /// <inheritdoc />
    public void ThrowIfInvalid(string path)
    {
        try
        {
            _provider.Load(path);
        }
        catch (Exception e)
        {
            throw new ValidationException($"Device [{path}] is invalid. Error: {e.Message}", e);
        }
    }
}