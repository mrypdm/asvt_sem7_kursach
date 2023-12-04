using System.ComponentModel.DataAnnotations;
using Devices.Providers;
using Devices.Validators;

namespace Devices.Tests;

public class DeviceValidatorTests
{
    [Test]
    public void ValidateCorrectDevice()
    {
        // Arrange

        var validator = new DeviceValidator(new DeviceProvider());

        // Act & Assert

        Assert.That(validator.Validate(Constants.DefaultDevice, out _), Is.True);
    }

    [Test]
    public void ValidateCorrectDeviceNotThrow()
    {
        // Arrange

        var validator = new DeviceValidator(new DeviceProvider());

        // Act & Assert

        validator.ThrowIfInvalid(Constants.DefaultDevice);
    }

    [Test]
    public void ValidateIncorrectDevice()
    {
        // Arrange

        var validator = new DeviceValidator(new DeviceProvider());

        // Act & Assert

        Assert.Multiple(() =>
        {
            Assert.That(validator.Validate(Constants.InvalidDevice, out var message), Is.False);
            Assert.That(message, Is.EqualTo("Cannot find devices"));
        });
    }

    [Test]
    public void ValidateIncorrectDeviceThrow()
    {
        // Arrange

        var validator = new DeviceValidator(new DeviceProvider());

        // Act

        var e = Assert.Throws<ValidationException>(() => validator.ThrowIfInvalid(Constants.InvalidDevice));

        // Assert

        Assert.That(e!.Message, Does.Contain("Cannot find devices"));
    }
}