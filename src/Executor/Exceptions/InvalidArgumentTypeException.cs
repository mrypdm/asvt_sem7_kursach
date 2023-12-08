namespace Executor.Exceptions;

public class InvalidArgumentTypeException : Exception
{
    public InvalidArgumentTypeException(IEnumerable<Type> expectedTypes, IEnumerable<Type> actualTypes)
        : base(
            "Invalid arguments types." +
            $"Expected: {string.Join(", ", expectedTypes.Select(t => t.Name))}." +
            $"But was: {string.Join(", ", actualTypes.Select(t => t.Name))}")
    {
    }
}