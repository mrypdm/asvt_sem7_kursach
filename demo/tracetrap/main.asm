; Программа демонстрации ловушки трассировки

START:
	MOV #HANDLER, @#20
	MOV #200, @#22
	MOV #TRACE_HANDLER, @#14
	MOV #200, @#16
	IOT
	INC R1
	INC R1
	HALT

HANDLER:
	MOV #20, 2(R6) ; set T flag in PSW of main program
	RTI

TRACE_HANDLER:
	INC R0
	RTI