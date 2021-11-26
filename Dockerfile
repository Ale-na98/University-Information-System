FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY project .
RUN ls -la /app
RUN dotnet restore M10.sln

# Copy everything else and build
#COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .

# Run the app on container startup
# Use your project name for the second parameter
# e.g. MyProject.dll
CMD ASPNETCORE\_URLS=http://\*:$PORT dotnet module_10.dll