param([string]$Path)

Import-Module 'C:\github\Compiler\KleinCmdlets\bin\Debug\KleinCmdlets.dll'
$result = Test-KleinProgram $Path
if($result -eq $true){
    "Valid Program"
} else {
    "Invalid Program"
}

