using System;

namespace Executor;

interface IMemory {
  ushort GetWord(ushort address);
  byte GetByte(ushort address);
  void SetWord(ushort address, ushort value);
  void SetByte(ushort address, byte value);
}

public class Memory: IMemory{
        private ushort[] RawMemory;

        public byte GetByte(ushort address){
            if (address % 2 == 0){
                return (byte)(RawMemory[address] >> 8);
            }
            return (byte)(RawMemory[address] & 255);
        }

        public ushort GetWord(ushort address) => RawMemory[address];

        public void SetByte(ushort address, byte value){
            if (address % 2 == 0){
                RawMemory[address] &= 255;
                RawMemory[address] |= (ushort)(value << 8);
                return;
            }
            RawMemory[address] &= (ushort)(255 << 8);
            RawMemory[address] |= value;
        }

        public byte SetWord(ushort address, ushort value){
            RawMemory[address] = value;
        }

        Memory(){
            RawMemory = new ushort[32513];
        }
               
}


