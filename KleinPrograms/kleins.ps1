param([string]$Path)

Import-Module ".\KleinCmdlets.dll"

Get-KleinTokens $Path | foreach {
    if($_.GetType().Name -eq 'ErrorToken') {
        Write-Host -ForegroundColor Red $_.ToString()
    } else {
        $_.ToString()
    }
}

