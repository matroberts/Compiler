Import-Module 'C:\github\Compiler\KleinCmdlets\bin\Debug\KleinCmdlets.dll'

Get-KleinTokens C:\github\Compiler\KleinPrograms\Programs\circular-prime.kln | foreach {
    if($_.GetType().Name -eq 'ErrorToken') {
        Write-Host -ForegroundColor Red $_.ToString()
    } else {
        $_.ToString()
    }
}

