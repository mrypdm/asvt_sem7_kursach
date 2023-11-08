using System;

namespace Executor;

interface IMemory {
  ushort GetWord(ushort address);
  byte GetByte(ushort address);
  void SetWord(ushort address, ushort value);
  void SetByte(ushort address, byte value);
}

public class Memory: IMemory{
        private ushort[] RawMemory = new ushort[65026];

        public byte GetByte(ushort address){
            return (byte)(RawMemory[address]);
        }

        public ushort GetWord(ushort address) {
            ushort value = 0;
            if (address % 2 == 1){
                throw new InvalidOperationException("Address is not odd. Only odd addresses allowed when getting word");
            }
            value |= (ushort)(RawMemory[address] << 8);
            value |= RawMemory[address+1];
            return value;
        }
        public void SetByte(ushort address, byte value){
            RawMemory[address] = value;
        }

        public void SetWord(ushort address, ushort value){
            if (address % 2 == 1){
                throw new InvalidOperationException("Address is not odd. Only odd addresses allowed when setting word");
            }
            RawMemory[address+1] = (byte)(value & 255);
            RawMemory[address] = (byte)(value >> 8);
        }

               
}


