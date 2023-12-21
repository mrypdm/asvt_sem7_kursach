using System;
using System.Collections.Generic;
using Assembler;
using Domain.Models;

namespace AssemblerApp;

internal static class Program
{
    public static void Main(string[] args)
    {
        var mainAsmFile = args[0];
        //var linkedAsmFiles = new List<string> { @"D:\Университет\7 семестр\Курсовая работы\macro.asm" };
        var linkedAsmFiles = new List<string>();

        var project = new Project
        {
            Executable = mainAsmFile,
            Files = linkedAsmFiles,
        };

        var asm = new Compiler();
        try
        {
            asm.Compile(project).Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}