FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7670

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["SecretaryTelegramAIBot.WebAPI.csproj", "Hosts/SecretaryTelegramAIBot.WebAPI/"]
COPY ["../../SecretaryTelegramAIBot.Application/SecretaryTelegramAIBot.Application.csproj", "SecretaryTelegramAIBot.Application/"]
COPY ["../../SecretaryTelegramAIBot.Domain/SecretaryTelegramAIBot.Domain.csproj", "SecretaryTelegramAIBot.Domain/"]
COPY ["../../SecretaryTelegramAIBot.Infrastructure/SecretaryTelegramAIBot.Infrastructure.csproj", "SecretaryTelegramAIBot.Infrastructure/"]

RUN dotnet restore "SecretaryTelegramAIBot.WebAPI.csproj"
RUN dotnet restore "../../SecretaryTelegramAIBot.Application/SecretaryTelegramAIBot.Application.csproj"
RUN dotnet restore "../../SecretaryTelegramAIBot.Domain/SecretaryTelegramAIBot.Domain.csproj"
RUN dotnet restore "../../SecretaryTelegramAIBot.Infrastructure/SecretaryTelegramAIBot.Infrastructure.csproj"

COPY . .
WORKDIR "/src/Hosts/SecretaryTelegramAIBot.WebAPI"
RUN dotnet build "./SecretaryTelegramAIBot.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SecretaryTelegramAIBot.WebAPI.csproj" -C $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecretaryTelegramAIBot.WebAPI.dll"]
