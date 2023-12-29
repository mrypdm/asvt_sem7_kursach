# Курсовая работа

Предмет: Аппаратные средства вычислительной техники

Тема: Программный симулятор PDP-11

Выполнили студенты ИУ8-71
- Тимощук Андрей Андреевич
- Шаповалов Максим Евгеньевич
- Штырков Василий Сергеевич

Москва, 2023

# Краткое описание

Симулятор состоит из трех модулей:
1. Ассемблер
2. Исполнитель
3. Графический интерфейс

### Контракт:

Через графический интерфейс или другим способом создается файл проекта с расширением `pdp11proj`.
Файл является JSON файлом со следующей схемой:

```JSON
{
    "Executable": "Путь к исполняемому файлу",
    "Files": ["Список файлов проекта"],
    "Devices": ["Список подключенных внешних устройств"],
    "StackAddress": "Начальный адрес указателя стека",
    "ProgramAddress": "Начальный адрес исполняемой программы в памяти"
}
```

Данный файл передается в Ассемблер, который компилирует исполняемый файл в объектный файл с раширением `pdp11bin`.
Структура объектного файла:

```
000000;command
000000'
000000
...
```

где `000000` - машинный код, `;command` - исходная строчка кода, `'` - метка перемещаемого адреса.

Файл проекта передается в Исполнитель, который считывает начальные адреса, подключенные внешние устройства и объектный файл.

### Внешние устройства

Для разработки внешних устройств релизован SDK, сотоящий из одного интерфейса (см. [SDK](https://github.com/mrypdm/asvt_sem7_kursach/tree/master/sdk/DeviceSdk))

### Графический интерфейс

Состоит из окна редактора и окна исполнителя.

Окно исполнителя содержит карту памяти, данные регистров, подключенные внешние устройства и исполняему программу.
Поддерживется автоматическое и пошаговое исполнение программы. Поддерживаются точки останова.

### Ограничения

Несмотря на то, что файл проект может состоять из нескольких файлов, данный функционал не реализован, т.к. по ходу согласования было решено, что он не нужен.
Возможно в будущем он все же будет имплементирован.

# Литература

- [Книга по PDP-11/VAX-11](https://www.google.com/search?q=vax%2Bpdp%2B11%2B%D1%83%D1%81%D1%82%D1%80%D0%BE%D0%B9%D1%81%D1%82%D0%B2%D0%BE%2Bsite%3A1801bm1.com&newwindow=1&client=ms-android-xiaomi&sxsrf=APwXEdfpa9g1i9m1TKaZAg35LOxJCwdw9g%3A1685034933938&ei=tZdvZOfuONO73AOcoZnIBA&oq=vax%2Bpdp%2B11%2B%D1%83%D1%81%D1%82%D1%80%D0%BE%D0%B9%D1%81%D1%82%D0%B2%D0%BE%2Bsite%3A1801bm1.com&gs_lcp=ChNtb2JpbGUtZ3dzLXdpei1zZXJwEAM6CggAEEcQ1gQQsAM6BQghEKABSgQIQRgAUPUEWNhNYKxPaAFwAHgAgAHfAYgBxRaSAQYwLjE2LjKYAQCgAQHAAQHIAQg&sclient=mobile-gws-wiz-serp)
- [PDP11-40 Processor Handbook](https://pdos.csail.mit.edu/6.828/2005/readings/pdp11-40.pdf)
