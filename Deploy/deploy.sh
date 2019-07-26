ApiKey=$1
Source=$2

dotnet nuget push ./ArchUnitNET/nupkgs/TngTech.ArchUnitNET.*.nupkg -k $ApiKey -s $Source