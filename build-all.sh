#!/bin/bash

echo "Building all projects with build version '${BuildVersion}'."

dotnet build './src/Docker.DotNet.Reactive*' --version-suffix $BuildVersion
