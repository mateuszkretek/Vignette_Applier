.CODE
calculateDistance proc
PSLLDQ xmm0, 8					;przesuniecie bitowe o 8 bajtów w lewo
VBLENDPD xmm0, xmm0, xmm1, 1	;przepisanie xmm1 do dolnych 8 bajtów rejestru xmm0
PSLLDQ xmm2, 8					;przesuniecie bitowe o 8 bajtów w lewo
VBLENDPD xmm1, xmm2, xmm3, 1	;przepisanie xmm3 do dolnych i xmm2 do górnych 8 bajtów rejestru xmm1
VSUBPD xmm0, xmm0, xmm1			;odejmowanie wektorowe xmm0-xmm1
VMULPD xmm0, xmm0, xmm0			;wektorowe podniesienie do kwadratu zawartosci xmm0
HADDPD xmm0, xmm0				;horyzontalne dodanie do siebie liczb w xmm0
VSQRTPD xmm0, xmm0				;wektorowy pierwiastek wartosci w xmm0 
ret
calculateDistance endp

calculateMaskValueAsm proc
CALL calculateDistance			;obliczenie dystansu pomiędzy danym punktem a środkiem winiety
MULSD xmm0, QWORD PTR[rsp+40]	;przemnożenie wartosci xmm0 aby znajdowała się w przedziale <0,Pi/2)
MOVSD QWORD PTR[rsp+40], xmm0	;przekazanie xmm0 na stos
FLD QWORD PTR[rsp+40]			;przekazanie ze stosu na stos FPU
FCOS							;obliczenie cosinusa
FSTP QWORD PTR[rsp+40]			;zdjęcie wartosci ze stosu FPU na stos
MOVSD xmm0, QWORD PTR[rsp+40]	;przekazanie wartosci do xmm0
MOV ecx, DWORD PTR [rsp+48]		;przekazanie moct winiety do licznika pętli
DEC ecx							;zmniejszenie licznika pętli
JZ power1						;skok jeśli licznik był jedynką
power:							
MULSD xmm0, QWORD PTR[rsp+40]	;potęgowanie
LOOPNE power					;pętla
power1:
ret
calculateMaskValueAsm endp
end

