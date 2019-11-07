#!/usr/bin/env bash
set -ev
dotnet restore
dotnet build -c Release
[[ -z "$TRAVIS_TAG" ]] && dotnet pack -c Release --output nupkgs -p:PackageVersion="0.0.0" -p:AssemblyVersion="0.0.0.0" || ([[ $TRAVIS_TAG =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]] && dotnet pack -c Release --output nupkgs -p:PackageVersion="$TRAVIS_TAG" -p:AssemblyVersion="$TRAVIS_TAG.0" || (echo "Git Tag has to resemble a package version (e.g. 1.0.0)." && exit 1 ))
dotnet test -c Release