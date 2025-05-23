FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ExternalOrderReportsService/ExternalOrderReportsService.csproj", "ExternalOrderReportsService/"]
#RUN dotnet nuget add source http://lk_emitter-nexus-1:8081/repository/nuget-hosted/

RUN dotnet restore "ExternalOrderReportsService/ExternalOrderReportsService.csproj"
COPY . .
WORKDIR "/src/ExternalOrderReportsService"
RUN dotnet build "ExternalOrderReportsService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExternalOrderReportsService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExternalOrderReportsService.dll"]