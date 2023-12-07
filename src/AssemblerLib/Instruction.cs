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
            {"clr", new Instruction("clr", 0b0_000_101_000_000_000, 1)},
            {"clrb", new Instruction("clrb", 0b1_000_101_000_000_000, 1)},
            {"com", new Instruction("com", 0b0_000_101_001_000_000, 1)},
            {"comb", new Instruction("comb", 0b1_000_101_001_000_000, 1)},
            {"inc", new Instruction("inc", 0b0_000_101_010_000_000, 1)},
            {"incb", new Instruction("incb", 0b1_000_101_010_000_000, 1)},
            {"dec", new Instruction("dec", 0b0_000_101_011_000_000, 1)},
            {"decb", new Instruction("decb", 0b1_000_101_011_000_000, 1)},
            {"neg", new Instruction("neg", 0b0_000_101_100_000_000, 1)},
            {"negb", new Instruction("negb", 0b1_000_101_100_000_000, 1)},
            {"tst", new Instruction("tst", 0b0_000_101_111_000_000, 1)},
            {"asr", new Instruction("asr", 0b0_000_110_010_000_000, 1)},
            {"asrb", new Instruction("asrb", 0b1_000_110_010_000_000, 1)},
            {"tstb", new Instruction("tstb", 0b1_000_101_111_000_000, 1)},
            {"asl", new Instruction("asl", 0b0_000_110_011_000_000, 1)},
            {"aslb", new Instruction("aslb", 0b1_000_110_011_000_000, 1)},
            {"ror", new Instruction("ror", 0b0_000_110_000_000_000, 1)},
            {"rorb", new Instruction("rorb", 0b1_000_110_000_000_000, 1)},
            {"rol", new Instruction("rol", 0b0_000_110_001_000_000, 1)},
            {"rolb", new Instruction("rolb", 0b1_000_110_001_000_000, 1)},
            {"swab", new Instruction("swab", 0b0_000_000_011_000_000, 1)},
            {"adc", new Instruction("adc", 0b0_000_101_101_000_000, 1)},
            {"adcb", new Instruction("adcb", 0b1_000_101_101_000_000, 1)},
            {"sbc", new Instruction("sbc", 0b0_000_101_110_000_000, 1)},
            {"sbcb", new Instruction("sbcb", 0b1_000_101_110_000_000, 1)},
            {"sxt", new Instruction("sbc", 0b0_000_110_111_000_000, 1)},
            {"mfps", new Instruction("mfps", 0b1_000_110_111_000_000, 1)},
            {"mtps", new Instruction("mtps", 0b1_000_110_100_000_000, 1)},
            {"mov", new Instruction("mov", 0b0_001_000_000_000_000, 2)},
            {"movb", new Instruction("movb", 0b1_001_000_000_000_000, 2)},
            {"cmp", new Instruction("cmp", 0b0_010_000_000_000_000, 2)},
            {"cmpb", new Instruction("cmpb", 0b1_010_000_000_000_000, 2)},
            {"add", new Instruction("add", 0b0_110_000_000_000_000, 2)},
            {"sub", new Instruction("sub", 0b1_110_000_000_000_000, 2)},
            {"bit", new Instruction("bit", 0b0_011_000_000_000_000, 2)},
            {"bitb", new Instruction("bitb", 0b1_011_000_000_000_000, 2)},
            {"bic", new Instruction("bic", 0b0_100_000_000_000_000, 2)},
            {"bicb", new Instruction("bicb", 0b1_100_000_000_000_000, 2)},
            {"bis", new Instruction("bis", 0b0_101_000_000_000_000, 2)},
            {"bisb", new Instruction("bisb", 0b1_101_000_000_000_000, 2)},
            {"xor", new Instruction("xor", 0b0_111_100_000_000_000, 2)},

            {"br", new Instruction("br", 0b0_000_000_100_000_000, 1)},
            {"bne", new Instruction("bne", 0b0_000_001_000_000_000, 1)},
            {"beq", new Instruction("beq", 0b0_000_001_100_000_000, 1)},
            {"bpl", new Instruction("bne", 0b1_000_000_000_000_000, 1)},
            {"bmi", new Instruction("bmi", 0b1_000_000_100_000_000, 1)},
            {"bvc", new Instruction("bvc", 0b1_000_010_000_000_000, 1)},
            {"bvs", new Instruction("bvs", 0b1_000_010_100_000_000, 1)},
            {"bcc", new Instruction("bcc", 0b1_000_011_000_000_000, 1)},
            {"bcs", new Instruction("bcs", 0b1_000_011_100_000_000, 1)},
            {"bge", new Instruction("bge", 0b0_000_010_000_000_000, 1)},
            {"blt", new Instruction("blt", 0b0_000_010_100_000_000, 1)},
            {"bgt", new Instruction("bgt", 0b0_000_011_000_000_000, 1)},
            {"ble", new Instruction("ble", 0b0_000_011_100_000_000, 1)},
            {"bhi", new Instruction("bhi", 0b1_000_001_000_000_000, 1)},
            {"blos", new Instruction("blos", 0b1_000_001_100_000_000, 1)},
            {"bhis", new Instruction("bhis", 0b1_000_011_000_000_000, 1)},
            {"blo", new Instruction("blo", 0b1_000_011_100_000_000, 1)},
            {"jmp", new Instruction("jmp", 0b0_000_000_001_000_000, 1)},
            {"jsr", new Instruction("jsr", 0b0_000_100_000_000_000, 2)},
            {"rts", new Instruction("rts", 0b0_000_000_010_000_000, 1)},
            {"mark", new Instruction("mark", 0b0_000_110_100_000_000, 1)},
            {"sob", new Instruction("sob", 0b0_111_111_000_000_000, 2)},

            {"bpt", new Instruction("bpt", 0b0_000_000_000_000_011, 0)},
            {"iot", new Instruction("iot", 0b0_000_000_000_000_100, 0)},
            {"rti", new Instruction("rti", 0b0_000_000_000_000_010, 0)},
            {"rtt", new Instruction("rtt", 0b0_000_000_000_000_110, 0)},
            {"halt", new Instruction("halt", 0b0_000_000_000_000_000, 0)},
            {"wait", new Instruction("wait", 0b0_000_000_000_000_001, 0)},
            {"reset", new Instruction("reset", 0b0_000_000_000_000_101, 0)}
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
