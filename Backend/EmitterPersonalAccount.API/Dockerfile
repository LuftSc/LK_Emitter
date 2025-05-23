FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
EXPOSE 5000

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EmitterPersonalAccount.API/EmitterPersonalAccount.API.csproj", "EmitterPersonalAccount.API/"]
#RUN dotnet nuget add source http://lk_emitter-nexus-1:8081/repository/nuget-hosted/

RUN dotnet restore "EmitterPersonalAccount.API/EmitterPersonalAccount.API.csproj"
COPY . .
WORKDIR "/src/EmitterPersonalAccount.API"
RUN dotnet build "EmitterPersonalAccount.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EmitterPersonalAccount.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmitterPersonalAccount.API.dll"]