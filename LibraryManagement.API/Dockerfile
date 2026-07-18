FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LibraryManagement.API/LibraryManagement.API.csproj", "LibraryManagement.API/"]
COPY ["LibraryManagement.Application/LibraryManagement.Application.csproj", "LibraryManagement.Application/"]
COPY ["LibraryManagement.Domain/LibraryManagement.Domain.csproj", "LibraryManagement.Domain/"]
COPY ["LibraryManagement.Infrastructure/LibraryManagement.Infrastructure.csproj", "LibraryManagement.Infrastructure/"]
RUN dotnet restore "LibraryManagement.API/LibraryManagement.API.csproj"
COPY . .
WORKDIR "/src/LibraryManagement.API"
RUN dotnet build "./LibraryManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LibraryManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibraryManagement.API.dll"]
