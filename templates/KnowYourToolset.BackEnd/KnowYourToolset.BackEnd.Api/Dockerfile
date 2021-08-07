#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["KnowYourToolset.BackEnd.Api/KnowYourToolset.BackEnd.Api.csproj", "KnowYourToolset.BackEnd.Api/"]
RUN dotnet restore "KnowYourToolset.Api.Api/KnowYourToolset.Api.csproj"
COPY . .
WORKDIR "/src/KnowYourToolset.BackEnd.Api"
RUN dotnet build "KnowYourToolset.BackEnd.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KnowYourToolset.BackEnd.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KnowYourToolset.BackEnd.Api.dll"]