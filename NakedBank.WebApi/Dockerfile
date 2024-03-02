#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/nightly/sdk:7.0.306-alpine3.17-arm64v8 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/nightly/sdk:7.0.306-alpine3.17-arm64v8 AS build
WORKDIR /src
COPY ["NakedBank.WebApi/NakedBank.WebApi.csproj", "NakedBank.WebApi/"]
RUN dotnet restore "NakedBank.WebApi/NakedBank.WebApi.csproj"
COPY . .
WORKDIR "/src/NakedBank.WebApi"
RUN dotnet build "NakedBank.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NakedBank.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NakedBank.WebApi.dll"]