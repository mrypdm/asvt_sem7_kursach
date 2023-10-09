using System;
namespace State{
    public record State {
        public ushort RAM; // aka Регистер адреса памяти
        public ushort MDR; // aka Регистр данных памяти
        public ushort RIP; // aka Instruction pointer
        public ushort[8] R;
    }
}
