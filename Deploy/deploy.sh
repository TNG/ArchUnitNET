#!/usr/bin/env bash
ApiKey=$1
Source=$2

echo "check"
dotnet nuget push ./ArchUnitNET/nupkgs/TngTech.ArchUnitNET.*.nupkg -k $ApiKey -s $Source