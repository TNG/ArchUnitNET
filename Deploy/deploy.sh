#!/usr/bin/env bash
ApiKey=$1
Source=$2

nuget pack ArchUnitNET.csproj
nuget push ./ArchUnitNET.*.nupkg -Verbosity detailed -ApiKey $ApiKey -Source $Source
git add ./ArchUnitNet/nupkgs/*
git add ./TestAssembly/nupkgs/*
