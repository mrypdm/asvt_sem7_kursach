namespace ROM;

/// <summary>
/// Errors for <see cref="RomDevice"/>
/// </summary>
public enum Error
{
    NoError,
    InvalidFunction,
    OddAddress,
    NoAddress,
    NoValue
}