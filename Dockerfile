FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Search.Api/Search.Api.csproj", "Search.Api/"]
COPY ["Search.Contract/Search.Contract.csproj", "Search.Contract/"]
COPY ["Search.Model/Search.Model.csproj", "Search.Model/"]
COPY ["Search.Service/Search.Service.csproj", "Search.Service/"]
RUN dotnet restore "Search.Api/Search.Api.csproj"
COPY . .
WORKDIR "/src/Search.Api"
RUN dotnet build "Search.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Search.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Search.Api.dll"]

