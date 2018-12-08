# Release Process
Build and package the project with the _Release_ configuration.

```
dotnet build .\src\MaybeSharp.sln -c Release
dotnet pack .\src\MaybeSharp.sln -c Release
```

Publish package with
```
dotnet nuget push src\MaybeSharp\bin\Release\MaybeSharp.[X.Y.Z].nupkg -k [API-Key] -s https://api.nuget.org/v3/index.json
```
