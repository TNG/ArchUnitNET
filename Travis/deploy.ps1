param(
[string]$apiKey,
[string]$source,
[string]$tag
)

dotnet nuget push ./ArchUnitNET/nupkgs/TngTech.ArchUnitNET.*.nupkg -k $apiKey -s $source
dotnet nuget push ./ArchUnitNET.xUnit/nupkgs/TngTech.ArchUnitNET.xUnit.*.nupkg -k $apiKey -s $source
dotnet nuget push ./ArchUnitNET.NUnit/nupkgs/TngTech.ArchUnitNET.NUnit.*.nupkg -k $apiKey -s $source
