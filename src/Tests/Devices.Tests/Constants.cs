using System.IO;

namespace Devices.Tests;

public static class Constants
{
    public static string DefaultDevice =>
        Path.Combine(Directory.GetCurrentDirectory(), "DemoDevices", "DemoDevice.dll");

    public static string DoubleDevice =>
        Path.Combine(Directory.GetCurrentDirectory(), "DemoDevices", "DoubleDevice.dll");

    public static string InvalidDevice =>
        Path.Combine(Directory.GetCurrentDirectory(), "DemoDevices", "InvalidDevice.dll");
}