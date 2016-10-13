param([string]$Path)

Import-Module '.\KleinCmdlets.dll'
# Program is output to current folder (not the source folder)
$out = ([Io.FileInfo]$Path).basename + ".tm"
Parse-KleinProgram $Path | Compile-KleinProgram | Out-File $out