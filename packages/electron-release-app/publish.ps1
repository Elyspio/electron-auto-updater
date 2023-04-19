$project = (Get-Childitem -Path $PSScriptRoot -Include *.csproj -Recurse).FullName;


function GetNextPackageVersion
{
    $content = (Get-Content -Path $project)

    $version = [regex]::Match($content, '<Version>(.*)</Version>').Groups[1].Value

    $split = $version.Split(".");

    $split[2] = ([int]$split[2]) + 1;

    return ($split -join ".");
}


$newVersion = GetNextPackageVersion;


$content = (Get-Content -Path $project);
$newContent = $content -replace "<Version>(.*)</Version>", "<Version>$newVersion</Version>";
Set-Content -Path $project -Value $newContent;

Write-Host "Modification de $project vers la version $newVersion";


cd $PSScriptRoot ; dotnet pack --configuration Release

$package = (Get-Childitem -Path $PSScriptRoot -Include *.nupkg -Recurse).FullName;

dotnet nuget push $package --source "https://nuget.pkg.github.com/Elyspio/index.json" --skip-duplicate --api-key dashlane

Remove-Item -Recurse Electron.Updater.Release.App/bin