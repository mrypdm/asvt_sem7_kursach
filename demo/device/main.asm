; Простая программа демонстрации работы с ВУ
; Включая демонстрацию прерываний
; см /sdk/ROM

START:
mov #HANDLER, @#60
mov #200, @#62

mov #16, @#177000
mov #7, @#177002

TRY1:
bit #200, @#177002
beq TRY1

mov #176641, @#177000
mov #11, @#177002

TRY2:
bit #200, @#177002
beq TRY2

mov #105, @#177002

TRY3:
jmp TRY3

HANDLER:
mov #1, R0
HALT