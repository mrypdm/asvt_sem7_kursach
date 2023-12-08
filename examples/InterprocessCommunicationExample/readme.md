## Пример общения двух процессов
Простой пример общения двух процессов с использованием
[NamedPipeServerStream](https://learn.microsoft.com/ru-ru/dotnet/api/system.io.pipes.namedpipeserverstream?view=net-7.0)
и
[NamedPipeClientStream](https://learn.microsoft.com/ru-ru/dotnet/api/system.io.pipes.namedpipeclientstream).

Может быть полезен для разработки внешних устройств (в частности для терминала, где необходим доступ к консоли и клавиатуре)