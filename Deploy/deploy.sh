ApiKey=$1
Source=$2

nuget pack ./ArchUnitNET/ArchUnitNET.nuspec -Verbosity detailed

nuget push ./DnDGen.ArchUnitNET.*.nupkg -Verbosity detailed -ApiKey $ApiKey -Source $Source
