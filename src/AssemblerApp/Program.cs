using System;
using System.Linq;
using Assembler;
using Assembler.Exceptions;
using Domain.Models;
using Domain.Providers;

namespace AssemblerApp;

internal static class Program
{
    public static void Main(string[] args)
    {
        var provider = new ProjectProvider();

        IProject project;

        try
        {
            project = provider.OpenProjectAsync(args[0]).Result;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Cannot open project file. Exception:\n{e.Message}");
            return;
        }

        try
        {
            new Compiler().Compile(project).Wait();
        }
        catch (AggregateException e) when (e.InnerExceptions.First() is AssembleException)
        {
            var ex = e.InnerExceptions.First() as AssembleException;
            Console.WriteLine($"Error at line [{ex.LineNumber}] [{ex.LineText}]. {ex.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Assembling ended with error:\n{e.Message}");
        }
    }
}