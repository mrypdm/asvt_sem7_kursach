﻿using TestCppSharedLibUsageCS;

using var builder = new CoolStringBuilder();

builder.Append("Hello").AppendLine(", world!").AppendLine("Пример 😀");

var str = builder.GetString();

Console.WriteLine(str);