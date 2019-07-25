ApiKey=$1
Source=$2


nuget push ./ArchUnitNET.*.nupkg -Verbosity detailed -ApiKey $ApiKey -Source $Source
