param(
[string]$apiKey,
[string]$source
)

dotnet nuget push ($PWD.Path + "\nupkgs\TngTech.ArchUnitNET.*.nupkg") -k $apiKey -s $source
