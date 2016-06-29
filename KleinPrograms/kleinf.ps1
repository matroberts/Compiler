param([string]$Path)

Import-Module '.\KleinCmdlets.dll'
$result = Test-KleinProgram $Path
if($result -ne $null){
    "Valid Program"
} else {
    "Invalid Program"
}

