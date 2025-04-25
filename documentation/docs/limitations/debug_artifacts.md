##Debug Artifacts
ArchUnitNET gathers information about the architecture from analyzing 
binaries, therefore running tests with the Release option (`dotnet test -c Release`) instead of the Debug
option (`dotnet test -c Debug`) can lead to not finding dependencies you normally would expect to find. 
The edge cases we found so far are not initializing a local variable, casting an object, using
the typeof() statement and checking that an async function is called.
A minimal example for each edge case can be found [here](https://github.com/TNG/ArchUnitNET/blob/master/ExampleTest/LimitationsOnReleaseTest.cs).


If you come across another edge case, where executing tests in Debug mode leads to different results than executing 
tests in Release mode, let us know via a [github issue](https://github.com/TNG/ArchUnitNET/issues).
 