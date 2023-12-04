namespace Devices.Validators;

/// <summary>
/// Validator for devices
/// </summary>
public interface IDeviceValidator
{
    /// <summary>
    /// Validates device
    /// </summary>
    /// <param name="path">Path to device</param>
    /// <returns>True if device is valid</returns>
    bool Validate(string path);
}