using System;
namespace State{
    public class State {
        public ushort ram; // aka Регистер адреса памяти
        public ushort rip; // aka Instruction pointer
        public ushort[8] r;
    }
}
