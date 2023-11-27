using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AssemblerLib
{
    internal class TokenBuilder
    {
        private const string RegexPatternAddrType0 = @"^r([0-7])$";
        private const string RegexPatternAddrType1 = @"^@r([0-7])$";
        private const string RegexPatternAddrType2 = @"^\(r([0-7])\)\+$";
        private const string RegexPatternAddrType3 = @"^@\(r([0-7])\)\+$";
        private const string RegexPatternAddrType4 = @"^-\(r([0-7])\)$";
        private const string RegexPatternAddrType5 = @"^@-\(r([0-7])\)$";
        private const string RegexPatternAddrType6 = @"^([0-1]*[0-7]{1,5})\(r([0-7])\)$";
        private const string RegexPatternAddrType7 = @"^@([0-1]*[0-7]{1,5})\(r([0-7])\)$";
        private const string RegexPatternAddrType21 = @"^#([0-1]*[0-7]{1,5})$";
        private const string RegexPatternAddrType31 = @"^@#([0-1]*[0-7]{1,5})$";
        private const string RegexPatternAddrType61 = @"^([a-z]+[_a-z0-9]*)$";
        private const string RegexPatternAddrType71 = @"^@([a-z]+[_a-z0-9]*)$";
        private readonly Regex _regexMaskAddrType0;
        private readonly Regex _regexMaskAddrType1;
        private readonly Regex _regexMaskAddrType2;
        private readonly Regex _regexMaskAddrType3;
        private readonly Regex _regexMaskAddrType4;
        private readonly Regex _regexMaskAddrType5;
        private readonly Regex _regexMaskAddrType6;
        private readonly Regex _regexMaskAddrType7;
        private readonly Regex _regexMaskAddrType21;
        private readonly Regex _regexMaskAddrType31;
        private readonly Regex _regexMaskAddrType61;
        private readonly Regex _regexMaskAddrType71;

        private const string RegexPatternArgNN = @"^([0-7]{1,2})$";
        private readonly Regex _regexMaskArgNN;

        private readonly Dictionary<string, Func<CommandLine, List<IToken>>> _instructions;

        private string ArgumentHandler(string arg, List<IToken> extraTokens)
        {
            string instArgMC = "";

            if (_regexMaskAddrType0.IsMatch(arg))
            {
                instArgMC += "0" + _regexMaskAddrType0.Match(arg).Groups[1].Value;
            }
            else if (_regexMaskAddrType1.IsMatch(arg))
            {
                instArgMC += "1" + _regexMaskAddrType1.Match(arg).Groups[1].Value;
            }
            else if (_regexMaskAddrType2.IsMatch(arg))
            {
                instArgMC += "2" + _regexMaskAddrType2.Match(arg).Groups[1].Value;
            }
            else if (_regexMaskAddrType3.IsMatch(arg))
            {
                instArgMC += "3" + _regexMaskAddrType2.Match(arg).Groups[1].Value;
            }
            else if (_regexMaskAddrType4.IsMatch(arg))
            {
                instArgMC += "4" + _regexMaskAddrType4.Match(arg).Groups[1].Value;
            }
            else if (_regexMaskAddrType5.IsMatch(arg))
            {
                instArgMC += "5" + _regexMaskAddrType5.Match(arg).Groups[1].Value;
            }
            else if (_regexMaskAddrType6.IsMatch(arg))
            {
                instArgMC += "6" + _regexMaskAddrType6.Match(arg).Groups[2].Value;
                // Generation extra token
                string extraWord = _regexMaskAddrType6.Match(arg).Groups[1].Value.PadLeft(6, '0');
                extraTokens.Add(new RawToken(extraWord));
            }
            else if (_regexMaskAddrType7.IsMatch(arg))
            {
                instArgMC += "7" + _regexMaskAddrType6.Match(arg).Groups[2].Value;
                // Generation extra token
                string extraWord = _regexMaskAddrType6.Match(arg).Groups[1].Value.PadLeft(6, '0');
                extraTokens.Add(new RawToken(extraWord));
            }
            else if (_regexMaskAddrType21.IsMatch(arg))
            {
                instArgMC += "27";
                // Generation extra token
                string extraWord = _regexMaskAddrType21.Match(arg).Groups[1].Value.PadLeft(6, '0');
                extraTokens.Add(new RawToken(extraWord));
            }
            else if (_regexMaskAddrType31.IsMatch(arg))
            {
                instArgMC += "37";
                // Generation extra token
                string extraWord = _regexMaskAddrType31.Match(arg).Groups[1].Value.PadLeft(6, '0');
                extraTokens.Add(new RawToken(extraWord));
            }
            else if (_regexMaskAddrType61.IsMatch(arg))
            {
                instArgMC += "67";
                // Generation extra token
                extraTokens.Add(new MarkRDToken(_regexMaskAddrType61.Match(arg).Groups[1].Value));
            }
            else if (_regexMaskAddrType71.IsMatch(arg))
            {
                instArgMC += "77";
                // Generation extra token
                extraTokens.Add(new MarkRDToken(_regexMaskAddrType61.Match(arg).Groups[1].Value));
            }
            else
            {
                throw new ArgumentException($"Incorrect argument: {arg}.");
            }

            return instArgMC;
        }

        private List<IToken> InstructionArgsNull(CommandLine cmdLine)
        {
            var resultTokens = new List<IToken>()
            {
                new OPToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code)
            };

            return resultTokens;
        }

        private List<IToken> InstructionArgsDD(CommandLine cmdLine)
        {
            var resultTokens = new List<IToken>();
            // Tokens for extra words
            var extraTokens = new List<IToken>();

            string instArgMC = ArgumentHandler(cmdLine.Arguments[0], extraTokens);

            resultTokens.Add(new OPToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code + instArgMC));
            resultTokens.AddRange(extraTokens);

            return resultTokens;
        }

        private List<IToken> InstructionArgsSSDD(CommandLine cmdLine)
        {
            var resultTokens = new List<IToken>();
            // Tokens for extra words
            var extraTokens = new List<IToken>();

            string instArgMC = "";
            instArgMC += ArgumentHandler(cmdLine.Arguments[0], extraTokens);
            instArgMC += ArgumentHandler(cmdLine.Arguments[1], extraTokens);

            resultTokens.Add(new OPToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code + instArgMC));
            resultTokens.AddRange(extraTokens);

            return resultTokens;
        }

        private List<IToken> InstructionArgsR(CommandLine cmdLine)
        {
            var resultTokens = new List<IToken>();
            string instArgMC = "";

            if (_regexMaskAddrType0.IsMatch(cmdLine.Arguments[0]))
            {
                instArgMC += _regexMaskAddrType0.Match(cmdLine.Arguments[0]).Groups[1].Value;
            }
            else
            {
                throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
            }

            resultTokens.Add(new OPToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code + instArgMC));
            return resultTokens;
        }

        private List<IToken> InstructionArgsRDD(CommandLine cmdLine)
        {
            var resultTokens = new List<IToken>();
            // Tokens for extra words
            var extraTokens = new List<IToken>();
            string instArgMC = "";

            if (_regexMaskAddrType0.IsMatch(cmdLine.Arguments[0]))
            {
                instArgMC += _regexMaskAddrType0.Match(cmdLine.Arguments[0]).Groups[1].Value;
            }
            else
            {
                throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
            }

            instArgMC += ArgumentHandler(cmdLine.Arguments[1], extraTokens);

            resultTokens.Add(new OPToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code + instArgMC));
            resultTokens.AddRange(extraTokens);

            return resultTokens;
        }

        private List<IToken> InstructionArgsNN(CommandLine cmdLine)
        {
            var resultTokens = new List<IToken>();
            string instArgMC = "";

            if (_regexMaskArgNN.IsMatch(cmdLine.Arguments[0]))
            {
                instArgMC += _regexMaskArgNN.Match(cmdLine.Arguments[0]).Groups[1].Value.PadLeft(2, '0');
            }
            else
            {
                throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
            }

            resultTokens.Add(new OPToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code + instArgMC));
            return resultTokens;
        }

        private List<IToken> InstructionArgsRNN(CommandLine cmdLine)
        {
            var resultTokens = new List<IToken>();
            string instArgMC = "";

            // The handling of the firct argument
            if (_regexMaskAddrType0.IsMatch(cmdLine.Arguments[0]))
            {
                instArgMC += _regexMaskAddrType0.Match(cmdLine.Arguments[0]).Groups[1].Value;
            }
            else
            {
                throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
            }

            // The handling of the second argument
            if (_regexMaskArgNN.IsMatch(cmdLine.Arguments[1]))
            {
                instArgMC += _regexMaskArgNN.Match(cmdLine.Arguments[1]).Groups[1].Value.PadLeft(2, '0');
            }
            else
            {
                throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[1]}.");
            }

            resultTokens.Add(new OPToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code + instArgMC));
            return resultTokens;
        }

        private List<IToken> InstructionArgsShift(CommandLine cmdLine)
        {
            var resultTokens = new List<IToken>();
            
            var arg = cmdLine.Arguments[0];
            if (_regexMaskAddrType61.IsMatch(arg))
            {
                resultTokens.Add(new OPShiftToken(
                    Instruction.Instructions[cmdLine.InstructionMnemonics].Code,
                    cmdLine.Arguments[0])
                    );
            }
            else
            {
                throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
            }

            return resultTokens;
        }

        public TokenBuilder()
        {
            _instructions = new Dictionary<string, Func<CommandLine, List<IToken>>>()
            {
                {"sob", InstructionArgsRNN},
                {"mark", InstructionArgsNN},
                {"rts", InstructionArgsR},
                {"br", InstructionArgsShift},
                {"xor", InstructionArgsRDD},
                {"mov", InstructionArgsSSDD},
                {"asr", InstructionArgsDD},
                {"halt", InstructionArgsNull}
            };

            _regexMaskAddrType0 = new Regex(RegexPatternAddrType0, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _regexMaskAddrType1 = new Regex(RegexPatternAddrType1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _regexMaskAddrType2 = new Regex(RegexPatternAddrType2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _regexMaskAddrType3 = new Regex(RegexPatternAddrType3, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _regexMaskAddrType4 = new Regex(RegexPatternAddrType4, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _regexMaskAddrType5 = new Regex(RegexPatternAddrType5, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _regexMaskAddrType6 = new Regex(RegexPatternAddrType6, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _regexMaskAddrType7 = new Regex(RegexPatternAddrType7, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _regexMaskAddrType21 = new Regex(RegexPatternAddrType21, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _regexMaskAddrType31 = new Regex(RegexPatternAddrType31, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _regexMaskAddrType61 = new Regex(RegexPatternAddrType61, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _regexMaskAddrType71 = new Regex(RegexPatternAddrType71, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            _regexMaskArgNN = new Regex(RegexPatternArgNN, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public List<IToken> Build(CommandLine cmdLine)
        {
            var resultTokens = new List<IToken>();
            resultTokens.AddRange(_instructions[cmdLine.InstructionMnemonics](cmdLine));

            return resultTokens;
        }
    }
}
