#!/usr/bin/env bash
set -ev
dotnet restore
dotnet build -c Release
[[ -z "$TRAVIS_TAG" ]] && dotnet pack -c Release --output nupkgs -p:PackageVersion="0.0.0" || dotnet pack -c Release --output nupkgs -p:PackageVersion="$TRAVIS_TAG"
dotnet test -c Release