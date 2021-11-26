FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

WORKDIR /app

COPY project .

RUN ls -la /app

RUN dotnet restore M10.sln

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /app

COPY --from=build-env /app/out .

CMD ["/bin/bash", "-c", "ASPNETCORE_URLS=http://0.0.0.0:$PORT dotnet module_10.dll"]