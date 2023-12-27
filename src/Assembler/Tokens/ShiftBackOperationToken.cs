using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Tokens
{
    internal class ShiftBackOperationToken : ShiftOperationToken
    {
        public ShiftBackOperationToken(CommandLine commandLine, int machineCode, string mark, int shiftMask,
            CommandLine originCmdLine) :
            base(commandLine, machineCode, mark, shiftMask, originCmdLine)
        {
        }

        public override IEnumerable<string> Translate(Dictionary<string, int> marksDict, int currentAddr)
        {
            int delta = 0;
            if (marksDict.TryGetValue(_mark, out var markAddress))
            {
                if (markAddress >= currentAddr)
                {
                    throw new Exception(
                        $"The instruction ({_originCmdLine.InstructionMnemonics}) can't uses forward marks ({_mark}).");
                }

                delta = currentAddr - markAddress;
            }
            else
            {
                throw new Exception($"The mark ({_mark}) is not determined.");
            }

            // Distance restrictions for the mark
            if (delta > _shiftMask)
            {
                throw new Exception($"The distance to the mark ({_mark}) is too large. {delta}");
            }

            var shiftValue = (delta / 2 + 1) & _shiftMask;

            return new List<string>
                { Convert.ToString(_machineCode | shiftValue, 8).PadLeft(6, '0') + $";{_originCmdLine.LineText}" };
        }
    }
}