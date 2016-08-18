*
* RUN-TIME SYSTEM
*
main() : integer
   secondary()

secondary() : integer
   1

* building a new stack frame.  addresses relative to  r6 which is the current top of the stack
* in this first version there are not arguments to pass

* calling sequence caller
*  evalueate arguments and store in stack frame
*  store return address in control link field
*  store current value of status in stack frame (well this is a register, so may as well store with all the other registers)
*  set value of status to at end of new stack frame


* CallingSequence_CallingProcedure
* store return address in control link field                                   
0: LDA 5, <ReturnAddressOffset>(7)             ; put return address in r5.  return address = <returnaddressoffset> + r7
1: ST 5, <StackControlLinkOffset>(6)           ; store the return link r5 in the stack.  stack position = <StackControlLinkOffset> + r6
* store current value of registers (including status) in stack frame
2: ST 0, <StackRegister0Offset>(6)             ; store r0 in stack. stack position = <StackRegister0Offset> + r6
3: ST 1, <StackRegister1Offset>(6)             ; store r0 in stack. stack position = <StackRegister1Offset> + r6
4: ST 2, <StackRegister2Offset>(6)             ; store r0 in stack. stack position = <StackRegister2Offset> + r6
5: ST 3, <StackRegister3Offset>(6)             ; store r0 in stack. stack position = <StackRegister3Offset> + r6
6: ST 4, <StackRegister4Offset>(6)             ; store r0 in stack. stack position = <StackRegister4Offset> + r6
7: ST 5, <StackRegister5Offset>(6)             ; store r0 in stack. stack position = <StackRegister5Offset> + r6
8: ST 6, <StackRegister6Offset>(6)             ; store r0 in stack. stack position = <StackRegister6Offset> + r6
* set value of status(r6) to new top of stack
9: LDA 6, <StackResister6Offset>(6)            ; StackRegister6Offset is the top of the stack
* jump to function
10: LDA 7, <AddressOfFunction>(0)               ; jump to function. i.e. load r7 with address of function (r0==0)
* callers return sequence <- return address

