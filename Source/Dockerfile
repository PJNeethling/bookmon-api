#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 80

RUN useradd -u 1000 ng
USER 1000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Bookmon.API/Bookmon.API.csproj"
RUN dotnet build "Bookmon.API/Bookmon.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bookmon.API/Bookmon.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookmon.API.dll"]