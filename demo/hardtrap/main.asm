; Программа демонстрации аппаратных ловушек (в данном случае нечетного адреса)

START:
	MOV #HANDLER, @#4
	MOV #200, @#6
	MOV #1, R7

RUNNER:
	MOV #1, R0
	HALT

HANDLER:
	MOV #RUNNER, @R6
	RTI