param(
[string]$apiKey,
[string]$source
)

dotnet nuget push ($PWD.Path + "\nupkgs\TngTech.ArchUnitNET.*.nupkg") -k $apiKey -s $source
dotnet nuget push ($PWD.Path + "\nupkgs\TngTech.ArchUnitNET.xUnit.*.nupkg") -k $apiKey -s $source
dotnet nuget push ($PWD.Path + "\nupkgs\TngTech.ArchUnitNET.NUnit.*.nupkg") -k $apiKey -s $source
dotnet nuget push ($PWD.Path + "\nupkgs\TngTech.ArchUnitNET.MSTestV2.*.nupkg") -k $apiKey -s $source
