ApiKey=$1
Source=$2

nuget pack ./ArchUnitNET/ArchUnitNET.nuspec -Verbosity detailed
nuget pack ./ArchUnitNETTests/Arch.nuspec -Verbosity detailed

nuget push ./DnDGen.TreasureGen.*.nupkg -Verbosity detailed -ApiKey $ApiKey -Source $Source
nuget push ./DnDGen.TreasureGen.Domain.*.nupkg -Verbosity detailed -ApiKey $ApiKey -Source $Source