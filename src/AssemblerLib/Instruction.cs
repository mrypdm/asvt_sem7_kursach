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
        private string _code;
        private byte _numVariables;
        
        public static readonly Dictionary<string, Instruction> Instructions = new Dictionary<string, Instruction>()
        {
            {"sob", new Instruction("sob", "007", 2)},
            {"mark", new Instruction("mark", "0064", 1)},
            {"rts", new Instruction("rts", "00020", 1)},
            {"br", new Instruction("br", "0001", 1)},
            {"bne", new Instruction("br", "0010", 1)},
            {"xor", new Instruction("xor", "074", 2)},
            {"asr", new Instruction("asr", "0062", 1)},
            {"mov", new Instruction("mov", "01", 2)},
            {"add", new Instruction("add", "06", 2)},
            {"halt", new Instruction("halt", "000000", 0)}
        };

        public Instruction(string mnemonics, string code, byte num_variables)
        {
            _mnemonics = mnemonics;
            _code = code;
            _numVariables = num_variables;
        }

        public string Code => _code;

        public byte ArgumentsCount => _numVariables;
    }
}
