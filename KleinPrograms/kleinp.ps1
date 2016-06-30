param([string]$Path)

Import-Module '.\KleinCmdlets.dll'
Parse-KleinProgram $Path | Format-PrettyAst
