#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/CleanTemplate.Auth/CleanTemplate.Auth.csproj", "src/CleanTemplate.Auth/"]
RUN dotnet restore "src/CleanTemplate.Auth/CleanTemplate.Auth.csproj"
COPY . .
WORKDIR "/src/src/CleanTemplate.Auth"
RUN dotnet build "CleanTemplate.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanTemplate.Auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanTemplate.Auth.dll"]