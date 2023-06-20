from cool_string_builder import CoolStringBuilder

builder = CoolStringBuilder()

builder.append("Hello").append_line(", world!").append_line("Example")

s = builder.get_string()

print(s)
