param(
[string]$apiKey,
[string]$source
)

dotnet nuget push ($PWD.Path + "\ArchUnitNET\nupkgs\TngTech.ArchUnitNET.*.nupkg") -k $apiKey -s $source
dotnet nuget push ($PWD.Path + "\ArchUnitNET.xUnit\nupkgs\TngTech.ArchUnitNET.xUnit.*.nupkg") -k $apiKey -s $source
dotnet nuget push ($PWD.Path + "\ArchUnitNET.NUnit\nupkgs\TngTech.ArchUnitNET.NUnit.*.nupkg") -k $apiKey -s $source
dotnet nuget push ($PWD.Path + "\ArchUnitNET.MSTestV2\nupkgs\TngTech.ArchUnitNET.MSTestV2.*.nupkg") -k $apiKey -s $source
