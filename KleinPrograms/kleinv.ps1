param([string]$Path)

Import-Module '.\KleinCmdlets.dll'
Parse-KleinProgram $Path | Get-SymbolTable | Format-Table
