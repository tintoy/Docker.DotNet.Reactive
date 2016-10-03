Param(
	[string] $BuildVersion = $null
)

If (!$BuildVersion) {
	$BuildVersion = 'dev'
}

$dotnet = Get-Command dotnet
& $dotnet build '.\src\Docker.DotNet.Reactive*' --version-suffix $BuildVersion
