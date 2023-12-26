; Программа демонстрации программных ловушек (в данном случае BPT)

START:
	MOV #HANDLER, @#14
	MOV #200, @#16
	BPT
	MOV #1, R0
	HALT
HANDLER:
	RTI