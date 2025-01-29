#!/bin/bash

# Run .NET tests with normal verbosity and filter out unnecessary xUnit logs
dotnet test CardValidation.Tests/CardValidation.Tests.csproj --logger "console;verbosity=normal" | \
grep -vE "xUnit.net|Starting|Discovered|Using reqnroll|Loading plugin"
