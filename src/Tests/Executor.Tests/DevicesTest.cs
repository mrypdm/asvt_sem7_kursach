using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Devices.Managers;
using Devices.Providers;
using Executor.States;
using Executor.Storages;
using Shared.Helpers;

namespace Executor.Tests;

public class DevicesTest
{
    [Test]
    public async Task SimpleTest()
    {
        // Arrange

        const ushort address = 14;
        const byte low = 0xA1;
        const byte high = 0xFD;
        const ushort expected = high << 8 | low;

        // CONTROL = 177002
        // BUFFER = 177000

        ushort[] rawMem =
        {
            Convert.ToUInt16("012737", 8), // 0 MOV #16, @#177000 ; address
            address, // 2
            Convert.ToUInt16("177000", 8), // 4

            Convert.ToUInt16("012737", 8), // 6 MOV #111, @#177002 ; set address
            Convert.ToUInt16("000107", 8), // 8
            Convert.ToUInt16("177002", 8), // 10

            Convert.ToUInt16("012737", 8), // 12 MOV #176641, @#177000 ; value
            expected, // 14
            Convert.ToUInt16("177000", 8), // 16

            Convert.ToUInt16("012737", 8), // 18 MOV #111, @#177002 ; set value
            Convert.ToUInt16("000111", 8), // 20
            Convert.ToUInt16("177002", 8), // 22

            Convert.ToUInt16("012737", 8), // 24 MOV #105, @#177002 ; write
            Convert.ToUInt16("000105", 8), // 26
            Convert.ToUInt16("177002", 8), // 28

            Convert.ToUInt16("000000", 8), // 30 HALT
        };

        var state = new State();
        var manager = new DevicesManager(new DeviceProvider());
        var bus = new Bus(new Memory(), manager);
        var opcodeIdentifier = new CommandParser(bus, state);

        for (var i = 0; i < rawMem.Length; i++)
        {
            bus.SetWord((ushort)(2 * i), rawMem[i]);
        }
        
        var devicePath = Path.Combine(Environment.CurrentDirectory, "Devices/ROM.dll");
        manager.Add(devicePath);
        var romDevice = manager.Devices.First();

        // Act

        romDevice.Init();

        while (state.Registers[7] != 30)
        {
            var word = bus.GetWord(state.Registers[7]);
            state.Registers[7] += 2;
            var command = opcodeIdentifier.GetCommand(word);
            command.Execute(command.GetArguments(word));

            if (state.Registers[7] is 12 or 24 or 30)
            {
                await TaskHelper.WaitForCondition(() => romDevice.HasInterrupt, TimeSpan.FromSeconds(10));
                romDevice.AcceptInterrupt();
            }
        }

        // Assert

        manager.Remove(devicePath);

        var bytes = await File.ReadAllBytesAsync("memory.bin");

        Assert.Multiple(() =>
        {
            Assert.That(bytes[address], Is.EqualTo(low));
            Assert.That(bytes[address + 1], Is.EqualTo(high));
        });
    }
}