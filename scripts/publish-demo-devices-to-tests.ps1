cd DemoDevice
dotnet publish -o ../../src/Tests/Devices.Tests/DemoDevices

cd ../DoubleDevice
dotnet publish -o ../../src/Tests/Devices.Tests/DemoDevices

cd ../InvalidDevice
dotnet publish -o ../../src/Tests/Devices.Tests/DemoDevices