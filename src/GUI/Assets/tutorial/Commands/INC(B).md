# INC(B)

Содержимое указанного операнда инкрементируется

Пример: INC ОП

Результат: ОП := ОП + 1

| Флаг | Значение                                 |
|------|------------------------------------------|
| N    | 1, если результат отрицательный, иначе 0 |
| Z    | 1, если результат равен 0, иначе 0       |
| V    | 1, если операнд равен 077777, иначе 0    |
| C    | Не изменяется                            |