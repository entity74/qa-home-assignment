# Use the .NET SDK (Make sure it's SDK, not ASP.NET Runtime)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set working directory
WORKDIR /app

# Copy everything to the container
COPY . . 

# Restore NuGet packages
RUN dotnet restore

# Build the project
RUN dotnet build --configuration Release --no-restore
