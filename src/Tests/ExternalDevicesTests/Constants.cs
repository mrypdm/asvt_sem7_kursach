using System.IO;

namespace ExternalDevicesTests;

public static class Constants
{
    public static string DefaultExternalDevice =>
        Path.Combine(Directory.GetCurrentDirectory(), "DemoExternalDevice", "DemoExternalDevice.dll");

    public static string DoubleExternalDevice =>
        Path.Combine(Directory.GetCurrentDirectory(), "DemoExternalDevice", "DoubleExternalDevice.dll");

    public static string InvalidExternalDevice =>
        Path.Combine(Directory.GetCurrentDirectory(), "DemoExternalDevice", "InvalidExternalDevice.dll");
}