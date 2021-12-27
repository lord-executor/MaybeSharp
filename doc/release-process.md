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

With the NuGet API key stored in the Windows credential manager, we can do
```
dotnet nuget push src\MaybeSharp\bin\Release\MaybeSharp.[X.Y.Z].nupkg -k $((Read-CredentialsStore -Target "NuGet:MaybeSharp:APIKey").GetNetworkCredential().Password) -s https://api.nuget.org/v3/index.json
```
