This little sample compiler converts programs written in
a simple language named ac into programs written in a
standard Unix desktop calculator language, dc.  You can
run the dc utility using the command "dc" at a command
prompt.

The files named *.ac are sample programs written in ac.
The files named *.dc are equivalent files written in dc.

The *.dc files were produced by the AcDc compiler from
the corresponding *.ac files.  You can see a trace of some
of the compilation process in the file acdc-examples.txt.

The Java code for the compiler is in the src/ directory.
AcDc.java is the "main program".  To read the compiler,
start with the file AcDc.java.  From there, you can work
your way through rest of the *.java files.  Likewise, to
compile the compiler, compile the file AcDc.java.  That
should compile the whole compiler.

I'll explain the language definitions for ac and dc in
our next class session, but I think you can get a feel
for the structure and operation of the compiler just
from the code and sample programs.

For next session, give most of your attention to the classes
in the "front end", the analysis part of the compiler.

Study the code, and come to class with a list of questions
you have!
