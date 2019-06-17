param([String]$file="DataContext.sql")

$regexA = '(CREATE PROCEDURE(.*?\s?)(?=END;))'
$regexB = '(ALTER PROCEDURE(.*?\s?)(?=END;))'
$encoding = New-Object System.Text.UTF8Encoding


Get-ChildItem  $file | % {
  $c = (Get-Content $_.FullName) -replace "`n"," " -replace "`r"," "
  [IO.File]::WriteAllText("$((Get-Item -Path ".\").FullName)\\"+$file, $c, $encoding)
}
Get-ChildItem $file | % {
  $c = (Get-Content $_.FullName) -replace $regexA,'EXEC(''$0'')'
  [IO.File]::WriteAllText("$((Get-Item -Path ".\").FullName)\\"+$file, $c, $encoding)
}
Get-ChildItem $file | % {
    $c = (Get-Content $_.FullName) -replace $regexB,'EXEC(''$0'')'
    [IO.File]::WriteAllText("$((Get-Item -Path ".\").FullName)\\"+$file, $c, $encoding)
  }
  Get-ChildItem $file | % {
    $c = (Get-Content $_.FullName) -replace '\s\s\s\s',"`r`n"
    [IO.File]::WriteAllText("$((Get-Item -Path ".\").FullName)\\"+$file, $c, $encoding)
  }
  Get-ChildItem $file | % {
    $c = (Get-Content $_.FullName) -replace 'GO',"`r`nGO`r`n"
    [IO.File]::WriteAllText("$((Get-Item -Path ".\").FullName)\\"+$file, $c, $encoding)
  }
Write-Host ($file+" has been fixed")