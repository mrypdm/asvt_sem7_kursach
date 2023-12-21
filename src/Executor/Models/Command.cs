namespace Executor.Models;

/// <summary>
/// Model of command line
/// </summary>
public record Command(ushort Address, ushort Value, bool BreakPoint, string Symbol);