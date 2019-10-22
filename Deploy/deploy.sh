#!/usr/bin/env bash
ApiKey=$1
Source=$2

dotnet nuget push ./ArchUnitNET/nupkgs/TngTech.ArchUnitNET.*.nupkg -k $ApiKey -s $Source
dotnet nuget push ./ArchUnitNET.xUnit/nupkgs/TngTech.ArchUnitNET.xUnit.*.nupkg -k $ApiKey -s $Source
dotnet nuget push ./ArchUnitNET.NUnit/nupkgs/TngTech.ArchUnitNET.NUnit.*.nupkg -k $ApiKey -s $Source