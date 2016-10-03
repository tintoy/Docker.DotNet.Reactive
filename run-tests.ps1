$dotnet = Get-Command dotnet

Function Invoke-DotNetTests([string] $ProjectName) {
	& $dotnet test ".\test\$ProjectName"
}

Invoke-DotNetTests -ProjectName Docker.DotNet.Reactive.Tests
Invoke-DotNetTests -ProjectName Docker.DotNet.Reactive.IntegrationTests
