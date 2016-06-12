param([string]$Path)

Import-Module 'C:\github\Compiler\KleinCmdlets\bin\Debug\KleinCmdlets.dll'

Parse-KleinProgram $Path
