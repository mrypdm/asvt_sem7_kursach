using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Assembler.Tokens;

namespace Assembler;

internal class TokenBuilder
{
    private const string RegexPatternAddrType0 = @"^r([0-7])$";
    private const string RegexPatternAddrType1 = @"^@r([0-7])$";
    private const string RegexPatternAddrType2 = @"^\(r([0-7])\)\+$";
    private const string RegexPatternAddrType3 = @"^@\(r([0-7])\)\+$";
    private const string RegexPatternAddrType4 = @"^-\(r([0-7])\)$";
    private const string RegexPatternAddrType5 = @"^@-\(r([0-7])\)$";
    private const string RegexPatternAddrType6 = @"^([0-1]*[0-7]{1,5})\(r([0-7])\)$";
    private const string RegexPatternAddrType6Mark = @"^([a-z]+[_a-z0-9]*)(([\+-])([0-1]*[0-7]{1,5}))?\(r([0-7])\)$";
    private const string RegexPatternAddrType7 = @"^@([0-1]*[0-7]{1,5})\(r([0-7])\)$";
    private const string RegexPatternAddrType7Mark = @"^@([a-z]+[_a-z0-9]*)(([\+-])([0-1]*[0-7]{1,5}))?\(r([0-7])\)$";
    private const string RegexPatternAddrType21 = @"^#([0-1]*[0-7]{1,5})$";
    private const string RegexPatternAddrType21Mark = @"^#([a-z]+[_a-z0-9]*)$";
    private const string RegexPatternAddrType31 = @"^@#([0-1]*[0-7]{1,5})$";
    private const string RegexPatternAddrType31Mark = @"^@#([a-z]+[_a-z0-9]*)$";
    private const string RegexPatternAddrType61 = @"^([a-z]+[_a-z0-9]*)$";
    private const string RegexPatternAddrType71 = @"^@([a-z]+[_a-z0-9]*)$";
    private const string RegexPatternArgNN = @"^([0-7]{1,2})$";
    private const string RegexPatternArgWORD = @"^([-]?[0-9]+)([.]?)$";
    private const string RegexPatternArgBLKW = @"^([0-9]+)$";

    private readonly Regex _regexMaskAddrType0;
    private readonly Regex _regexMaskAddrType1;
    private readonly Regex _regexMaskAddrType2;
    private readonly Regex _regexMaskAddrType3;
    private readonly Regex _regexMaskAddrType4;
    private readonly Regex _regexMaskAddrType5;
    private readonly Regex _regexMaskAddrType6;
    private readonly Regex _regexMaskAddrType6Mark;
    private readonly Regex _regexMaskAddrType7;
    private readonly Regex _regexMaskAddrType7Mark;
    private readonly Regex _regexMaskAddrType21;
    private readonly Regex _regexMaskAddrType21Mark;
    private readonly Regex _regexMaskAddrType31;
    private readonly Regex _regexMaskAddrType31Mark;
    private readonly Regex _regexMaskAddrType61;
    private readonly Regex _regexMaskAddrType71;
    private readonly Regex _regexMaskArgNN;
    private readonly Regex _regexMaskArgWORD;
    private readonly Regex _regexMaskArgBLKW;

    private readonly Dictionary<string, Func<CommandLine, List<IToken>>> _instructions;

    private int ArgumentHandler(string arg, List<IToken> extraTokens)
    {
        int instArgCode;

        if (_regexMaskAddrType0.IsMatch(arg))
        {
            instArgCode = 0b000_000;
            instArgCode = instArgCode | int.Parse(_regexMaskAddrType0.Match(arg).Groups[1].Value);
        }
        else if (_regexMaskAddrType1.IsMatch(arg))
        {
            instArgCode = 0b001_000;
            instArgCode = instArgCode | int.Parse(_regexMaskAddrType1.Match(arg).Groups[1].Value);
        }
        else if (_regexMaskAddrType2.IsMatch(arg))
        {
            instArgCode = 0b010_000;
            instArgCode = instArgCode | int.Parse(_regexMaskAddrType2.Match(arg).Groups[1].Value);
        }
        else if (_regexMaskAddrType3.IsMatch(arg))
        {
            instArgCode = 0b011_000;
            instArgCode = instArgCode | int.Parse(_regexMaskAddrType3.Match(arg).Groups[1].Value);
        }
        else if (_regexMaskAddrType4.IsMatch(arg))
        {
            instArgCode = 0b100_000;
            instArgCode = instArgCode | int.Parse(_regexMaskAddrType4.Match(arg).Groups[1].Value);
        }
        else if (_regexMaskAddrType5.IsMatch(arg))
        {
            instArgCode = 0b101_000;
            instArgCode = instArgCode | int.Parse(_regexMaskAddrType5.Match(arg).Groups[1].Value);
        }
        else if (_regexMaskAddrType6.IsMatch(arg))
        {
            instArgCode = 0b110_000;
            instArgCode = instArgCode | int.Parse(_regexMaskAddrType6.Match(arg).Groups[2].Value);

            // Generation extra token
            var extraWordCode = Convert.ToInt32(_regexMaskAddrType6.Match(arg).Groups[1].Value, 8);
            extraTokens.Add(new RawToken(extraWordCode));
        }
        else if (_regexMaskAddrType6Mark.IsMatch(arg))
        {
            instArgCode = 0b110_000;
            instArgCode = instArgCode | int.Parse(_regexMaskAddrType6Mark.Match(arg).Groups[5].Value);

            // Generation extra token
            var mark = _regexMaskAddrType6Mark.Match(arg).Groups[1].Value;
            var parseValue = _regexMaskAddrType6Mark.Match(arg).Groups[4].Value;
            var num = string.IsNullOrEmpty(parseValue) ? 0 : Convert.ToInt32(parseValue, 8);
            var opSign = _regexMaskAddrType6Mark.Match(arg).Groups[3].Value;
            extraTokens.Add(new MarkRelocationToken(mark, num, opSign == "+" ? true : false));
        }
        else if (_regexMaskAddrType7.IsMatch(arg))
        {
            instArgCode = 0b111_000;
            instArgCode = instArgCode | int.Parse(_regexMaskAddrType7.Match(arg).Groups[2].Value);

            // Generation extra token
            var extraWordCode = Convert.ToInt32(_regexMaskAddrType7.Match(arg).Groups[1].Value, 8);
            extraTokens.Add(new RawToken(extraWordCode));
        }
        else if (_regexMaskAddrType7Mark.IsMatch(arg))
        {
            instArgCode = 0b111_000;
            instArgCode = instArgCode | int.Parse(_regexMaskAddrType7Mark.Match(arg).Groups[5].Value);

            // Generation extra token
            var mark = _regexMaskAddrType7Mark.Match(arg).Groups[1].Value;
            var parseValue = _regexMaskAddrType7Mark.Match(arg).Groups[4].Value;
            var num = string.IsNullOrEmpty(parseValue) ? 0 : Convert.ToInt32(parseValue, 8);
            var opSign = _regexMaskAddrType7Mark.Match(arg).Groups[3].Value;
            extraTokens.Add(new MarkRelocationToken(mark, num, opSign == "+" ? true : false));
        }
        else if (_regexMaskAddrType21.IsMatch(arg))
        {
            instArgCode = 0b010_111;

            // Generation extra token
            var extraWordCode = Convert.ToInt32(_regexMaskAddrType21.Match(arg).Groups[1].Value, 8);
            extraTokens.Add(new RawToken(extraWordCode));
        }
        else if (_regexMaskAddrType21Mark.IsMatch(arg))
        {
            instArgCode = 0b010_111;

            // Generation extra token
            var mark = _regexMaskAddrType21Mark.Match(arg).Groups[1].Value;
            extraTokens.Add(new MarkRelocationToken(mark, 0, true));
        }
        else if (_regexMaskAddrType31.IsMatch(arg))
        {
            instArgCode = 0b011_111;

            // Generation extra token
            var extraWordCode = Convert.ToInt32(_regexMaskAddrType31.Match(arg).Groups[1].Value, 8);
            extraTokens.Add(new RawToken(extraWordCode));
        }
        else if (_regexMaskAddrType31Mark.IsMatch(arg))
        {
            instArgCode = 0b011_111;

            // Generation extra token
            var mark = _regexMaskAddrType31Mark.Match(arg).Groups[1].Value;
            extraTokens.Add(new MarkRelocationToken(mark, 0, true));
        }
        else if (_regexMaskAddrType61.IsMatch(arg))
        {
            instArgCode = 0b110_111;

            // Generation extra token
            extraTokens.Add(new MarkRelatedToken(_regexMaskAddrType61.Match(arg).Groups[1].Value));
        }
        else if (_regexMaskAddrType71.IsMatch(arg))
        {
            instArgCode = 0b111_111;

            // Generation extra token
            extraTokens.Add(new MarkRelatedToken(_regexMaskAddrType61.Match(arg).Groups[1].Value));
        }
        else
        {
            throw new ArgumentException($"Incorrect argument: {arg}.");
        }

        return instArgCode;
    }

    private List<IToken> InstructionArgsNull(CommandLine cmdLine)
    {
        return new List<IToken>
        {
            new OperationToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code, cmdLine)
        };
    }

    private List<IToken> InstructionArgsDD(CommandLine cmdLine)
    {
        var resultTokens = new List<IToken>();
        // Tokens for extra words
        var extraTokens = new List<IToken>();

        var instArgCode = ArgumentHandler(cmdLine.Arguments[0], extraTokens);

        resultTokens.Add(new OperationToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code | instArgCode, cmdLine));
        resultTokens.AddRange(extraTokens);

        return resultTokens;
    }

    private List<IToken> InstructionArgsSSDD(CommandLine cmdLine)
    {
        var resultTokens = new List<IToken>();
        // Tokens for extra words
        var extraTokens = new List<IToken>();

        var instArgCode = ArgumentHandler(cmdLine.Arguments[0], extraTokens);
        instArgCode = instArgCode << 6;
        instArgCode = instArgCode | ArgumentHandler(cmdLine.Arguments[1], extraTokens);

        resultTokens.Add(new OperationToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code | instArgCode, cmdLine));
        resultTokens.AddRange(extraTokens);

        return resultTokens;
    }

    private List<IToken> InstructionArgsR(CommandLine cmdLine)
    {
        var resultTokens = new List<IToken>();
        int instArgCode = 0;

        if (_regexMaskAddrType0.IsMatch(cmdLine.Arguments[0]))
        {
            instArgCode = Convert.ToInt32(_regexMaskAddrType0.Match(cmdLine.Arguments[0]).Groups[1].Value, 8);
        }
        else
        {
            throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
        }

        resultTokens.Add(new OperationToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code | instArgCode, cmdLine));
        return resultTokens;
    }

    private List<IToken> InstructionArgsRDD(CommandLine cmdLine)
    {
        var resultTokens = new List<IToken>();
        // Tokens for extra words
        var extraTokens = new List<IToken>();
        int instArgCode = 0;

        if (_regexMaskAddrType0.IsMatch(cmdLine.Arguments[0]))
        {
            instArgCode = Convert.ToInt32(_regexMaskAddrType0.Match(cmdLine.Arguments[0]).Groups[1].Value, 8);
            instArgCode = instArgCode << 6;
        }
        else
        {
            throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
        }

        instArgCode = instArgCode | ArgumentHandler(cmdLine.Arguments[1], extraTokens);

        resultTokens.Add(new OperationToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code | instArgCode, cmdLine));
        resultTokens.AddRange(extraTokens);

        return resultTokens;
    }

    private List<IToken> InstructionArgsNN(CommandLine cmdLine)
    {
        var resultTokens = new List<IToken>();
        int instArgCode = 0;

        if (_regexMaskArgNN.IsMatch(cmdLine.Arguments[0]))
        {
            instArgCode = Convert.ToInt32(_regexMaskArgNN.Match(cmdLine.Arguments[0]).Groups[1].Value, 8);
        }
        else
        {
            throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
        }

        resultTokens.Add(new OperationToken(Instruction.Instructions[cmdLine.InstructionMnemonics].Code | instArgCode, cmdLine));
        return resultTokens;
    }

    private List<IToken> InstructionArgsRNN(CommandLine cmdLine)
    {
        var resultTokens = new List<IToken>();
        int instArgCode = 0;

        // The handling of the firct argument
        if (_regexMaskAddrType0.IsMatch(cmdLine.Arguments[0]))
        {
            instArgCode = Convert.ToInt32(_regexMaskAddrType0.Match(cmdLine.Arguments[0]).Groups[1].Value, 8);
            instArgCode = instArgCode << 6;
        }
        else
        {
            throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
        }

        // The handling of the second argument
        if (_regexMaskAddrType61.IsMatch(cmdLine.Arguments[1]))
        {
            resultTokens.Add(new ShiftBackOperationToken(
                Instruction.Instructions[cmdLine.InstructionMnemonics].Code | instArgCode,
                cmdLine.Arguments[1],
                0b111_111,
                cmdLine));
        }
        else
        {
            throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[1]}.");
        }
        
        return resultTokens;
    }

    private List<IToken> InstructionArgsShift(CommandLine cmdLine)
    {
        var resultTokens = new List<IToken>();

        var arg = cmdLine.Arguments[0];
        if (_regexMaskAddrType61.IsMatch(arg))
        {
            resultTokens.Add(new ShiftOperationToken(
                Instruction.Instructions[cmdLine.InstructionMnemonics].Code,
                cmdLine.Arguments[0],
                0b1111_1111,
                cmdLine)
            );
        }
        else
        {
            throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
        }

        return resultTokens;
    }

    private List<IToken> PseudoInstructionWORD(CommandLine cmdLine)
    {
        var resultTokens = new List<IToken>();

        if (_regexMaskArgWORD.IsMatch(cmdLine.Arguments[0]))
        {
            var value = _regexMaskArgWORD.Match(cmdLine.Arguments[0]).Groups[1].Value;

            int valueDec;
            if (string.IsNullOrEmpty(_regexMaskArgWORD.Match(cmdLine.Arguments[0]).Groups[2].Value))
            {
                // .WORD N - oct-num
                var isNegative = value.StartsWith('-');
                valueDec = (isNegative ? -1 : 1) * Convert.ToInt32(isNegative ? value[1..] : value, 8);
            }
            else
            {
                // .WORD N. - dec-num
                valueDec = Convert.ToInt32(value);
            }

            if (valueDec is > short.MaxValue or < short.MinValue)
            {
                throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
            }

            valueDec &= 0xFFFF;

            resultTokens.Add(new RawToken(valueDec));
        }
        else
        {
            throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
        }

        return resultTokens;
    }

    private List<IToken> PseudoInstructionBLKW(CommandLine cmdLine)
    {
        var resultTokens = new List<IToken>();

        if (_regexMaskArgBLKW.IsMatch(cmdLine.Arguments[0]))
        {
            var valueDec = Convert.ToInt32(_regexMaskArgBLKW.Match(cmdLine.Arguments[0]).Groups[1].Value, 8);
            for (var i = 0; i < valueDec; i++)
            {
                resultTokens.Add(new RawToken(0));
            }
        }
        else
        {
            throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
        }

        return resultTokens;
    }

    private List<IToken> PseudoInstructionEND(CommandLine cmdLine)
    {
        var resultTokens = new List<IToken>();

        if (_regexMaskAddrType61.IsMatch(cmdLine.Arguments[0]))
        {
            resultTokens.Add(new MarkRelocationToken(_regexMaskAddrType61.Match(cmdLine.Arguments[0]).Groups[1].Value,
                0, true));
        }
        else
        {
            throw new ArgumentException($"Incorrect argument: {cmdLine.Arguments[0]}.");
        }

        return resultTokens;
    }

    public TokenBuilder()
    {
        _instructions = new Dictionary<string, Func<CommandLine, List<IToken>>>
        {
            { "clr", InstructionArgsDD },
            { "clrb", InstructionArgsDD },
            { "com", InstructionArgsDD },
            { "comb", InstructionArgsDD },
            { "inc", InstructionArgsDD },
            { "incb", InstructionArgsDD },
            { "dec", InstructionArgsDD },
            { "decb", InstructionArgsDD },
            { "neg", InstructionArgsDD },
            { "negb", InstructionArgsDD },
            { "tst", InstructionArgsDD },
            { "tstb", InstructionArgsDD },
            { "asr", InstructionArgsDD },
            { "asrb", InstructionArgsDD },
            { "asl", InstructionArgsDD },
            { "aslb", InstructionArgsDD },
            { "ror", InstructionArgsDD },
            { "rorb", InstructionArgsDD },
            { "rol", InstructionArgsDD },
            { "rolb", InstructionArgsDD },
            { "swab", InstructionArgsDD },
            { "adc", InstructionArgsDD },
            { "adcb", InstructionArgsDD },
            { "sbc", InstructionArgsDD },
            { "sbcb", InstructionArgsDD },
            { "sxt", InstructionArgsDD },
            { "mfps", InstructionArgsDD },
            { "mtps", InstructionArgsDD },

            { "mov", InstructionArgsSSDD },
            { "movb", InstructionArgsSSDD },
            { "cmp", InstructionArgsSSDD },
            { "cmpb", InstructionArgsSSDD },
            { "add", InstructionArgsSSDD },
            { "sub", InstructionArgsSSDD },
            { "bit", InstructionArgsSSDD },
            { "bitb", InstructionArgsSSDD },
            { "bic", InstructionArgsSSDD },
            { "bicb", InstructionArgsSSDD },
            { "bis", InstructionArgsSSDD },
            { "bisb", InstructionArgsSSDD },

            { "xor", InstructionArgsRDD },

            { "br", InstructionArgsShift },
            { "bne", InstructionArgsShift },
            { "beq", InstructionArgsShift },
            { "bpl", InstructionArgsShift },
            { "bmi", InstructionArgsShift },
            { "bvc", InstructionArgsShift },
            { "bvs", InstructionArgsShift },
            { "bcc", InstructionArgsShift },
            { "bcs", InstructionArgsShift },
            { "bge", InstructionArgsShift },
            { "blt", InstructionArgsShift },
            { "bgt", InstructionArgsShift },
            { "ble", InstructionArgsShift },
            { "bhi", InstructionArgsShift },
            { "blos", InstructionArgsShift },
            { "bhis", InstructionArgsShift },
            { "blo", InstructionArgsShift },

            { "jmp", InstructionArgsDD },
            { "jsr", InstructionArgsRDD },
            { "rts", InstructionArgsR },

            { "mark", InstructionArgsNN },
            { "sob", InstructionArgsRNN },

            { "bpt", InstructionArgsNull },
            { "iot", InstructionArgsNull },
            { "rti", InstructionArgsNull },
            { "rtt", InstructionArgsNull },
            { "halt", InstructionArgsNull },
            { "wait", InstructionArgsNull },
            { "reset", InstructionArgsNull },

            // Pseudo instructions
            { ".word", PseudoInstructionWORD },
            { ".blkw", PseudoInstructionBLKW },
            { ".end", PseudoInstructionEND }
        };

        _regexMaskAddrType0 = new Regex(RegexPatternAddrType0, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType1 = new Regex(RegexPatternAddrType1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType2 = new Regex(RegexPatternAddrType2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType3 = new Regex(RegexPatternAddrType3, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType4 = new Regex(RegexPatternAddrType4, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType5 = new Regex(RegexPatternAddrType5, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType6 = new Regex(RegexPatternAddrType6, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType6Mark =
            new Regex(RegexPatternAddrType6Mark, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType7 = new Regex(RegexPatternAddrType7, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType7Mark =
            new Regex(RegexPatternAddrType7Mark, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType21 = new Regex(RegexPatternAddrType21, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType21Mark =
            new Regex(RegexPatternAddrType21Mark, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType31 = new Regex(RegexPatternAddrType31, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType31Mark =
            new Regex(RegexPatternAddrType31Mark, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType61 = new Regex(RegexPatternAddrType61, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskAddrType71 = new Regex(RegexPatternAddrType71, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskArgNN = new Regex(RegexPatternArgNN, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        _regexMaskArgWORD = new Regex(RegexPatternArgWORD, RegexOptions.IgnoreCase | RegexOptions.Singleline);
        _regexMaskArgBLKW = new Regex(RegexPatternArgBLKW, RegexOptions.IgnoreCase | RegexOptions.Singleline);
    }

    public IEnumerable<IToken> Build(CommandLine cmdLine)
    {
        var resultTokens = new List<IToken>();
        resultTokens.AddRange(_instructions[cmdLine.InstructionMnemonics](cmdLine));
        return resultTokens;
    }
}