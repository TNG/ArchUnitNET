#!/usr/bin/env bash
set -ev
dotnet build ArchUnitNET -c Release --framework netstandard2.0
dotnet build ArchUnitNET.NUnit -c Release --framework netstandard2.0
dotnet build ArchUnitNET.xUnit -c Release --framework netstandard2.0
dotnet test -c Release --framework netcoreapp2.2
