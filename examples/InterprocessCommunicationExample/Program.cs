using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Terminal;

public static class Program
{
    private const string PipeName = "MainToChildPipe";

    private static string _otherSideName;
    private static PipeStream _pipe;

    private static readonly CancellationTokenSource TokenSource = new();
    private static readonly List<IDisposable> Resources = new();

    public static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "child")
        {
            InitChild();
        }
        else
        {
            InitMain();
        }

        Resources.Add(_pipe);
        Resources.Add(TokenSource);

        var readerTask = ReaderTask();
        var writerTask = WriterTask();

        writerTask.Wait();
        TokenSource.Cancel();
        readerTask.Wait();

        Resources.ForEach(r => r.Dispose());
    }

    /// <summary>
    /// If running as Main Terminal
    /// </summary>
    private static void InitMain()
    {
        Console.Title = "Main Terminal";
        _otherSideName = "child";

        var pipe = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte,
            PipeOptions.Asynchronous);

        var process = new Process();

        if (OperatingSystem.IsLinux())
        {
            // Linux has many different terminal shells,
            // so this solution is not general, but it's good as an example.
            process.StartInfo.FileName = "gnome-terminal";
            process.StartInfo.Arguments = "-- ./Terminal child";
        }
        else if (OperatingSystem.IsWindows())
        {
            process.StartInfo.FileName = "Terminal";
            process.StartInfo.Arguments = "child";
            process.StartInfo.UseShellExecute = true;
        }
        else
        {
            Console.WriteLine($"Unsupported OS: {Environment.OSVersion.Platform}");
        }

        process.Start();

        Console.Write("Waiting for connection... ");
        pipe.WaitForConnection();
        Console.WriteLine("Connected");

        Resources.Add(process);

        _pipe = pipe;
    }

    /// <summary>
    /// If running as Child Terminal
    /// </summary>
    private static void InitChild()
    {
        Console.Title = "Child Terminal";
        _otherSideName = "parent";

        var pipe = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
        pipe.Connect();
        _pipe = pipe;
    }

    /// <summary>
    /// Task for reading stdin and sending messages to other side
    /// </summary>
    private static async Task WriterTask()
    {
        await using var writer = new StreamWriter(_pipe);

        try
        {
            while (_pipe.IsConnected && !TokenSource.IsCancellationRequested)
            {
                var line = await ConsoleHelper.ReadLineAsync(TokenSource.Token);

                if (line == "!shutdown")
                {
                    break;
                }

                await writer.WriteLineAsync(line);
                await writer.FlushAsync();
            }
        }
        catch (Exception) when (!_pipe.IsConnected || TokenSource.IsCancellationRequested)
        {
            // Ignore
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Writer] [ERR] [{e.Message}]");
        }
    }

    /// <summary>
    /// Task for reading messages from other side and writing them to stdout
    /// </summary>
    private static async Task ReaderTask()
    {
        try
        {
            using var reader = new StreamReader(_pipe);
            var buffer = new char[1024];
            while (_pipe.IsConnected && !TokenSource.IsCancellationRequested)
            {
                var count = await StreamHelper.ReadAsync(reader, buffer, TokenSource.Token);

                if (count == 0)
                {
                    Console.WriteLine("Child disconnected. Shutting down.");
                    TokenSource.Cancel();
                    return;
                }

                Console.Write($"({_otherSideName}): ");
                Console.Write(buffer, 0, count);
            }
        }
        catch (Exception) when (!_pipe.IsConnected || TokenSource.IsCancellationRequested)
        {
            // Ignore
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Reader] [ERR] [{e.Message}]");
        }
    }
}