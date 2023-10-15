using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Terminal;

public static class ConsoleHelper
{
    public const string ShutdownRequest = "!shutdown";
    
    public static async Task<string> ReadLineAsync(CancellationToken token)
    {
        ConsoleKeyInfo keyInfo = default;
        var builder = new StringBuilder();

        Console.TreatControlCAsInput = true;

        do
        {
            token.ThrowIfCancellationRequested();

            if (!Console.KeyAvailable)
            {
                await Task.Delay(50, token);
                continue;
            }

            keyInfo = Console.ReadKey(true);

            if ((keyInfo.Modifiers & ConsoleModifiers.Alt) == ConsoleModifiers.Alt)
            {
                continue;
            }

            if ((keyInfo.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control)
            {
                if (keyInfo.Key == ConsoleKey.C)
                {
                    Console.WriteLine("!shutdown");
                    return ShutdownRequest;
                }

                continue;
            }

            if (keyInfo.KeyChar == '\u0000')
            {
                continue;
            }

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                continue;
            }

            if (keyInfo.Key == ConsoleKey.Tab)
            {
                continue;
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (builder.Length >= 1)
                {
                    var (column, row) = Console.GetCursorPosition();

                    if (column == row && row == 0)
                    {
                        continue;
                    }

                    if (--column == -1)
                    {
                        column = Console.WindowWidth - 1;
                        row = Math.Max(--row, 0);
                    }

                    builder.Remove(builder.Length - 1, 1);

                    Console.SetCursorPosition(column, row);
                    Console.Write(' ');
                    Console.SetCursorPosition(column, row);
                }

                continue;
            }

            Console.Write(keyInfo.KeyChar);
            builder.Append(keyInfo.KeyChar);
        } while (keyInfo.Key != ConsoleKey.Enter);

        Console.Write(Environment.NewLine);

        return builder.ToString().TrimEnd('\r', '\n');
    }
}

public static class StreamHelper
{
    public static async Task<int> ReadAsync(this StreamReader reader, char[] buffer, CancellationToken token)
    {
        var readingTask = reader.ReadAsync(buffer, 0, buffer.Length);

        var cancellationTask = Task.Delay(-1, token);

        await Task.WhenAny(readingTask, cancellationTask);

        token.ThrowIfCancellationRequested();

        return await readingTask;
    }
}