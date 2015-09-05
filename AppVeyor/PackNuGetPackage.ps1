$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$version = [System.Reflection.Assembly]::LoadFile("$root\Goji\bin\Release\Goji.dll").GetName().Version
$versionStr = "{0}.{1}.{2}.{3}" -f ($version.Major, $version.Minor, $version.Build, $version.Revision)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\AppVeyor\Goji.nuspec) 
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\AppVeyor\Goji.nuspec

& $root\AppVeyor\NuGet.exe pack $root\AppVeyor\Goji.nuspec -OutputDirectory $root\Goji\bin\Release\

Push-AppveyorArtifact Goji\bin\Release\Goji.*.nupkg
