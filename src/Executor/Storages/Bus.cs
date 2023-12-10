using System.Collections.Generic;
using System.Linq;
using Devices.Managers;
using Executor.Exceptions;

namespace Executor.Storages;

public class Bus : IStorage
{
    private readonly IStorage _storage;
    private readonly IDevicesManager _deviceManager;

    public Bus(IStorage storage, IDevicesManager deviceManager)
    {
        _storage = storage;
        _deviceManager = deviceManager;
    }

    public IReadOnlyCollection<byte> Data => _storage.Data;

    public ushort GetWord(ushort address)
    {
        if (address < _storage.Data.Count)
        {
            return _storage.GetWord(address);
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
        if (address < _storage.Data.Count)
        {
            return _storage.GetByte(address);
        }

        var wordAddress = address - (address % 2 == 1 ? 1 : 0);

        var device = _deviceManager.Devices.SingleOrDefault(d =>
            d.BufferRegisterAddress == wordAddress || d.ControlRegisterAddress == wordAddress);

        if (device != null)
        {
            var shift = address % 2 == 1 ? 8 : 0;
            if (wordAddress == device.BufferRegisterAddress)
            {
                return (byte)((device.BufferRegisterAddress >> shift) & 0xFF);
            }

            return (byte)((device.ControlRegisterValue >> shift) & 0xFF);
        }

        throw new BusException($"Cannot find address '{address}'");
    }

    public void SetWord(ushort address, ushort value)
    {
        if (address < _storage.Data.Count)
        {
            _storage.SetWord(address, value);
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
        if (address < _storage.Data.Count)
        {
            _storage.SetByte(address, value);
            return;
        }

        var wordAddress = address - (address % 2 == 1 ? 1 : 0);

        var device = _deviceManager.Devices.SingleOrDefault(d =>
            d.BufferRegisterAddress == wordAddress || d.ControlRegisterAddress == wordAddress);

        if (device != null)
        {
            var shift = address % 2 == 1 ? 8 : 0;
            var bitsToKeep = 0xFF << (8 - shift); // if odd 0xFF; if even 0xFF00

            if (wordAddress == device.BufferRegisterAddress)
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