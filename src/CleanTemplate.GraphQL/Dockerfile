#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
# Install Auth Certificate
# https://askgif.com/blog/154/how-to-install-trusted-root-certificates-in-dotnet-core-on-ubuntu/
COPY ["src/CleanTemplate.GraphQL/CleanTemplateServerCert.crt", "/usr/local/share/ca-certificates/"]
RUN update-ca-certificates

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/CleanTemplate.GraphQL/CleanTemplate.GraphQL.csproj", "src/CleanTemplate.GraphQL/"]
COPY ["src/CleanTemplate.Application/CleanTemplate.Application.csproj", "src/CleanTemplate.Application/"]
COPY ["src/CleanTemplate.Domain/CleanTemplate.Domain.csproj", "src/CleanTemplate.Domain/"]
COPY ["src/CleanTemplate.Infrastructure/CleanTemplate.Infrastructure.csproj", "src/CleanTemplate.Infrastructure/"]
RUN dotnet restore "src/CleanTemplate.GraphQL/CleanTemplate.GraphQL.csproj"
COPY . .
WORKDIR "/src/src/CleanTemplate.GraphQL"
RUN dotnet build "CleanTemplate.GraphQL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanTemplate.GraphQL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanTemplate.GraphQL.dll"]