using System;
using System.IO;
using System.Threading.Tasks;
using DeviceSdk;
using ROM;
using Shared.Helpers;

namespace Devices.Tests.DemoDevicesTests;

public class RomTests
{
    [TearDown]
    public void TearDown()
    {
        File.Delete("memory.bin");
    }

    [Test]
    public void InitTest()
    {
        // Arrange

        using var device = new RomDevice();

        // Act

        device.Init();

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(File.Exists("memory.bin"), Is.True);
            Assert.That(new FileInfo("memory.bin"), Has.Length.EqualTo(ushort.MaxValue));
            Assert.That(device.ControlRegisterValue & 128, Is.Not.Zero);
        });
    }

    [Test]
    public async Task ReadTest()
    {
        // Arrange

        const ushort address = 14;
        const byte low = 0xA1;
        const byte high = 0xFD;
        const ushort expected = (high << 8) | low;

        var buf = new byte[ushort.MaxValue];
        buf[address] = low;
        buf[address + 1] = high;
        await File.WriteAllBytesAsync("memory.bin", buf);

        using var device = new RomDevice();
        device.Init();

        // Act

        device.BufferRegisterValue = address;
        device.ControlRegisterValue = 0b0_00011_1; // set address & enable
        await WaitForReady(device);

        device.ControlRegisterValue = 0b0_00001_1; // read
        await WaitForReady(device);

        var actual = device.BufferRegisterValue;

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(GetError(device), Is.EqualTo(Error.NoError));
        });
    }

    [Test]
    public async Task WriteTest()
    {
        // Arrange

        const ushort address = 14;
        const byte low = 0xA1;
        const byte high = 0xFD;
        const ushort expected = high << 8 | low;

        var device = new RomDevice();
        device.Init();

        // Act

        device.BufferRegisterValue = address;
        device.ControlRegisterValue = 0b0_00011_1; // set address
        await WaitForReady(device);

        device.BufferRegisterValue = expected;
        device.ControlRegisterValue = 0b0_00100_1; // set value
        await WaitForReady(device);

        device.ControlRegisterValue = 0b0_00010_1; // write
        await WaitForReady(device);

        device.Dispose(); // manually because we need access to file

        var file = await File.ReadAllBytesAsync("memory.bin");

        var actual = file[address + 1] << 8 | file[address];

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(GetError(device), Is.EqualTo(Error.NoError));
        });
    }

    [Test]
    public async Task InvalidFunctionTest()
    {
        // Arrange

        using var device = new RomDevice();
        device.Init();

        // Act

        device.ControlRegisterValue = 0b0_10000_1;
        await WaitForReady(device);

        // Assert

        Assert.That(GetError(device), Is.EqualTo(Error.InvalidFunction));
    }

    [Test]
    [TestCase(0b0_00001_1)]
    [TestCase(0b0_00010_1)]
    public async Task NoAddressTest(int function)
    {
        // Arrange

        using var device = new RomDevice();
        device.Init();

        // Act

        device.ControlRegisterValue = (ushort)function;
        await WaitForReady(device);

        // Assert

        Assert.That(GetError(device), Is.EqualTo(Error.NoAddress));
    }

    [Test]
    public async Task NoValueTest()
    {
        // Arrange

        using var device = new RomDevice();
        device.Init();

        // Act

        device.BufferRegisterValue = 0;
        device.ControlRegisterValue = 0b0_00011_1; // set address
        await WaitForReady(device);

        device.ControlRegisterValue = 0b0_00010_1;
        await WaitForReady(device);

        // Assert

        Assert.That(GetError(device), Is.EqualTo(Error.NoValue));
    }

    [Test]
    public async Task OddAddressTest()
    {
        // Arrange

        using var device = new RomDevice();
        device.Init();

        // Act

        device.BufferRegisterValue = 15;
        device.ControlRegisterValue = 0b0_00011_1;
        await WaitForReady(device);

        // Assert

        Assert.That(GetError(device), Is.EqualTo(Error.OddAddress));
    }

    private static Error GetError(IDevice device) => (Error)((device.ControlRegisterValue & 0xF000) >> 12);

    private static Task WaitForReady(IDevice device) =>
        TaskHelper.WaitForCondition(() => (device.ControlRegisterValue & 0x80) != 0,
            period: TimeSpan.FromMilliseconds(50));
}