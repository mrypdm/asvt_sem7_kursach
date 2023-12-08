using System.IO;

namespace Domain.Tests;

public static class Constants
{
    public static readonly string CurrentDir = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}";
    
    public static readonly string ProjectDir = $"{CurrentDir}Projects{Path.DirectorySeparatorChar}";
}