# Use the official .NET SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project files and restore dependencies
COPY ThreadApi.csproj .
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime image as a runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory
WORKDIR /app

# Copy the published files from the build stage
COPY --from=build /app/out .

# Expose the port on which the application will run
EXPOSE 8080

# Set the entry point for the application
ENTRYPOINT ["dotnet", "ThreadApi.dll"]
