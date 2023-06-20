using TestCppSharedLibUsageCS;

var builder = new CoolStringBuilder();

builder.Append("Hello").AppendLine(", world!").AppendLine("Example");

var str = builder.GetString();

Console.WriteLine(str);