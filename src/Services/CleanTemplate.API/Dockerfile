#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Presentation/CleanTemplate.API/CleanTemplate.API.csproj", "src/Presentation/CleanTemplate.API/"]
COPY ["src/Infrastructure/CleanTemplate.Infrastructure/CleanTemplate.Infrastructure.csproj", "src/Infrastructure/CleanTemplate.Infrastructure/"]
COPY ["src/Core/CleanTemplate.Application/CleanTemplate.Application.csproj", "src/Core/CleanTemplate.Application/"]
COPY ["src/Core/CleanTemplate.SharedKernel/CleanTemplate.SharedKernel.csproj", "src/Core/CleanTemplate.SharedKernel/"]
RUN dotnet restore "src/Presentation/CleanTemplate.API/CleanTemplate.API.csproj"
COPY . .
WORKDIR "/src/src/Presentation/CleanTemplate.API"
RUN dotnet build "CleanTemplate.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanTemplate.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanTemplate.API.dll"]