*
* run-time system
*
0: LDC 7,4(0) ; Branch to function main at line number 4
1: LD 4,1023(0) load main's return value address in to register 4
2: OUT 4,0,0 ; print program return value
3: HALT 0,0,0 ; END IT
*
* main
*
4: LDC 1,1(0) ; Load CONSTANT 1 into register 1
5: ST 1,1021(0) ; Store register 1 into DMEM[1021]
6: OUT 1,0,0 ; Print register 1, the latest allocated register for print expression
7: ST 1,1023(0) ; Store return value in register 1 in main's return value address 1023
8: LDC 7,1(0) ; Branch back to run-time system
9: LDC 2,1(0) ; Load CONSTANT 1 into register 2
10: ST 2,1020(0) ; Store register 2 into DMEM[1020]
