FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Greentube.Weather/Greentube.Weather.csproj", "Greentube.Weather/"]
RUN dotnet restore "Greentube.Weather/Greentube.Weather.csproj"
COPY . .
WORKDIR "/src/Greentube.Weather"
RUN dotnet build "Greentube.Weather.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Greentube.Weather.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Greentube.Weather.dll"]
