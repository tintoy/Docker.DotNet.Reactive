#!/bin/bash

if [ "$TRAVIS_BUILD_NUMBER" != "" ] && [ "$DDR_BUILD_VERSION" != "" ]; then
	BuildVersion="${DDR_BUILD_VERSION}-${TRAVIS_BUILD_NUMBER}"
else
	BuildVersion="dev"
fi

echo "Building all packages with build version '${BuildVersion}'."

projects=`ls -d1 ./src/Docker.DotNet.Reactive*`
for project in $projects; do
	echo "Packing \"$project\"."
	dotnet pack "$project" --version-suffix $BuildVersion
done

echo "Done."
