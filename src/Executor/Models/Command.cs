namespace Executor.Models;

/// <summary>
/// Model of command
/// </summary>
/// <param name="Address">Address of command</param>
/// <param name="Code">Machine code of command</param>
/// <param name="BreakPoint">Is break point set</param>
/// <param name="Symbol">Symbol of command</param>
public record Command(ushort Address, ushort Code, bool BreakPoint, string Symbol);