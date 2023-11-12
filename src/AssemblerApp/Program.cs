using AssemblerLib;
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var mainAsmFile = @"D:\Университет\7 семестр\Курсовая работы\main2.asm";
        //var linkedAsmFiles = new List<string> { @"D:\Университет\7 семестр\Курсовая работы\macro.asm" };
        var linkedAsmFiles = new List<string>();

        var asm = new Assembler(mainAsmFile, linkedAsmFiles);
        try
        {
            asm.Assemble().Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
