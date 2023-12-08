using System.Linq;
using Devices.Managers;
using Executor.Exceptions;
using Executor.Memories;

namespace Executor.Buses;

public class Bus : IBus
{
    private readonly IMemory _memory;
    private readonly IDevicesManager _deviceManager;

    public Bus(IMemory memory, IDevicesManager deviceManager)
    {
        _memory = memory;
        _deviceManager = deviceManager;
    }

    public ushort GetWord(ushort address)
    {
        if (address < _memory.Data.Count)
        {
            return _memory.GetWord(address);
        }

        var device = _deviceManager.Devices.SingleOrDefault(d =>
            d.BufferRegisterAddress == address || d.ControlRegisterAddress == address);

        if (device != null)
        {
            return address == device.BufferRegisterAddress ? device.BufferRegisterValue : device.ControlRegisterValue;
        }

        throw new BusException($"Cannot find address '{address}'");
    }

    public byte GetByte(ushort address)
    {
        if (address < _memory.Data.Count)
        {
            return _memory.GetByte(address);
        }

        var device = _deviceManager.Devices.SingleOrDefault(d =>
            d.BufferRegisterAddress == address || d.ControlRegisterAddress == address);

        if (device != null)
        {
            var shift = address % 2 == 1 ? 8 : 0;

            if (address == device.BufferRegisterAddress)
            {
                return (byte)((device.BufferRegisterAddress >> shift) & 0xFF);
            }

            return (byte)((device.ControlRegisterValue >> shift) & 0xFF);
        }

        throw new BusException($"Cannot find address '{address}'");
    }

    public void SetWord(ushort address, ushort value)
    {
        if (address < _memory.Data.Count)
        {
            _memory.SetWord(address, value);
            return;
        }

        var device = _deviceManager.Devices.SingleOrDefault(d =>
            d.BufferRegisterAddress == address || d.ControlRegisterAddress == address);

        if (device != null)
        {
            if (address == device.BufferRegisterAddress)
            {
                device.BufferRegisterValue = value;
                return;
            }

            device.ControlRegisterValue = value;
            return;
        }

        throw new BusException($"Cannot find address '{address}'");
    }

    public void SetByte(ushort address, byte value)
    {
        if (address < _memory.Data.Count)
        {
            _memory.SetByte(address, value);
            return;
        }

        // theoretically, it will not be possible to use byte operations with odd addresses for devices,
        // because all their addresses are even
        // but just in case we will support it

        var device = _deviceManager.Devices.SingleOrDefault(d =>
            d.BufferRegisterAddress == address || d.ControlRegisterAddress == address);

        if (device != null)
        {
            var shift = address % 2 == 1 ? 8 : 0;
            var bitsToKeep = 0xFF << (8 - shift); // if odd 0xFF; if even 0xFF00

            if (address == device.BufferRegisterAddress)
            {
                device.BufferRegisterValue = (ushort)((device.BufferRegisterAddress & bitsToKeep) | (value << shift));
                return;
            }

            device.ControlRegisterValue = (ushort)((device.ControlRegisterValue & bitsToKeep) | (value << shift));
            return;
        }

        throw new BusException($"Cannot find address '{address}'");
    }
}