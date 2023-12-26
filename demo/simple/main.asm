; Простая программа демонстрации ветвлений, функций логики и псевдо-инструкций

START:
    sub #6, D   ; D = D - 6
    bmi LESS    ; if result < 0
    beq LESS    ; if result == 0
    ; D > 6 => F = A or B
    mov A, R0   ; R0 = A
    bis B, R0   ; R0 = R0 or B
    mov R0, F   ; F = R0
    jmp GLOBAL_END

LESS:
    ; D <= 6 => A and not B
    mov A, R0   ; R0 = A
    bic B, R0   ; R0 = ~B and R0
    mov R0, F   ; F = R0

GLOBAL_END:
    halt

D: .word 1
A: .word 6
B: .word 3
F: .blkw 1