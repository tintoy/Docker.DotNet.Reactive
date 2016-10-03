#!/bin/bash

echo "Running all tests..."

testProjects=`ls -d1 ./test/Docker.DotNet.Reactive.*`
for testProject in $testProjects; do
	echo "Running tests for project \"$testProject\"."
	dotnet test "$testProject"
done

echo "Done."
