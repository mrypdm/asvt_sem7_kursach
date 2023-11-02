using Devices.Providers;
using Devices.Validators;

namespace DevicesTests;

public class DeviceValidatorTests
{
    [Test]
    public void ValidateCorrectDevice()
    {
        // Arrange
        var manager = new DeviceValidator(new DeviceProvider());

        // Act & Assert
        Assert.That(manager.Validate(Constants.DefaultDevice), Is.True);
    }

    [Test]
    public void ValidateIncorrectDevice()
    {
        // Arrange
        var manager = new DeviceValidator(new DeviceProvider());

        // Act & Assert
        Assert.That(manager.Validate(Constants.InvalidDevice), Is.False);
    }
}