.code
calculateDistance proc
PSLLDQ xmm0, 8
VBLENDPD xmm0, xmm0, xmm1, 1
PSLLDQ xmm2, 8
VBLENDPD xmm1, xmm2, xmm3, 1
VSUBPD xmm0, xmm0, xmm1
VMULPD xmm0, xmm0, xmm0
HADDPD xmm0, xmm0
VSQRTPD xmm0, xmm0
ret
calculateDistance endp

calculateMaskValue proc
CALL calculateDistance
MULSD xmm0, QWORD PTR[rsp+40]
MOVSD QWORD PTR[rsp+40], xmm0
FLD QWORD PTR[rsp+40]
FCOS
FSTP QWORD PTR[rsp+40]
MOVSD xmm0, QWORD PTR[rsp+40]
MOVSD xmm1, QWORD PTR[rsp+40]
MOV ecx, DWORD PTR [rsp+48]
DEC ecx
power:
MULSD xmm0, xmm1
loop power
ret
calculateMaskValue endp
end