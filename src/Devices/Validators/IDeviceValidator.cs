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
    /// <param name="errorMessage">Fail reason</param>
    /// <returns>True if device is valid</returns>
    bool Validate(string path, out string errorMessage);

    /// <summary>
    /// Throws if device is ivalid
    /// </summary>
    /// <param name="path">Path to device</param>
    void ThrowIfInvalid(string path);
}