using AssemblerLib;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var mainAsmFile = @"D:\Университет\7 семестр\Курсовая работы\main.asm";
        var linkedAsmFiles = new List<string> { @"D:\Университет\7 семестр\Курсовая работы\macro.asm" };

        var asm = new Assembler(mainAsmFile, linkedAsmFiles);
        asm.Assemble().Wait();
    }
}
