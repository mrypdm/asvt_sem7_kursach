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
        
        public static readonly Dictionary<string, Instruction> instructions = new Dictionary<string, Instruction>()
        {
            {"asr", new Instruction("asr", "062{0}", 1)},
            {"mov", new Instruction("mov", "01{0}{1}", 2)},
            {"add", new Instruction("add", "06{0}{1}", 2)},
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
