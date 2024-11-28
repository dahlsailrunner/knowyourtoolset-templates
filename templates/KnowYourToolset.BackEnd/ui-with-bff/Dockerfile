FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
RUN mkdir /certs && \
    curl -o /certs/realpageintermediate-1.crt https://artifacts.realpage.com/artifactory/rp-nondeployables-local/certs/realpageintermediate-1.crt && \
    curl -o /certs/realpageintermediate-2.crt https://artifacts.realpage.com/artifactory/rp-nondeployables-local/certs/realpageintermediate-2.crt && \    
    curl -o /certs/rprootca.crt https://artifacts.realpage.com/artifactory/rp-nondeployables-local/certs/rprootca.crt
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["bff/*", "bff/"]
RUN dotnet restore "./bff/Project.Name.Bff.csproj"

FROM build AS publish
WORKDIR "/src/bff"
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Project.Name.Bff.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

## the copy of .dist folder assumes the angular build was done first
FROM base AS final
WORKDIR /app
COPY --from=publish /certs /usr/local/share/ca-certificates
RUN update-ca-certificates
COPY --from=publish /app/publish .
COPY dist /app/wwwroot 
ENTRYPOINT ["dotnet", "Project.Name.Bff.dll"]
