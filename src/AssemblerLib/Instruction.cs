using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerLib
{
    internal record Instruction
    {
        private string _mnemonics;
        private int _code;
        private byte _numVariables;
        
        public static readonly Dictionary<string, Instruction> Instructions = new Dictionary<string, Instruction>()
        {
            {"sob", new Instruction("sob", 0b0_111_111_000_000_000, 2)},
            {"mark", new Instruction("mark", 0b0_000_110_100_000_000, 1)},
            {"rts", new Instruction("rts", 0b0_000_000_010_000_000, 1)},
            {"br", new Instruction("br", 0b0_000_000_100_000_000, 1)},
            {"bne", new Instruction("bne", 0b0_000_001_000_000_000, 1)},
            {"xor", new Instruction("xor", 0b0_111_100_000_000_000, 2)},
            {"asr", new Instruction("asr", 0b0_000_110_010_000_000, 1)},
            {"mov", new Instruction("mov", 0b0_001_000_000_000_000, 2)},
            {"add", new Instruction("add", 0b1_110_000_000_000_000, 2)},
            {"halt", new Instruction("halt", 0b0_000_000_000_000_000, 0)}
        };

        public Instruction(string mnemonics, int code, byte num_variables)
        {
            _mnemonics = mnemonics;
            _code = code;
            _numVariables = num_variables;
        }

        public int Code => _code;

        public byte ArgumentsCount => _numVariables;
    }
}
