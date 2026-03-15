FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CSEMcp/CSEMcp.csproj", "CSEMcp/"]
COPY ["CSEMcp.Application/CSEMcp.Application.csproj", "CSEMcp.Application/"]
COPY ["CSEMcp.Domain/CSEMcp.Domain.csproj", "CSEMcp.Domain/"]
COPY ["CSEMcp.Infrastructure/CSEMcp.Infrastructure.csproj", "CSEMcp.Infrastructure/"]
RUN dotnet restore "CSEMcp/CSEMcp.csproj"
COPY . .
WORKDIR "/src/CSEMcp"
RUN dotnet build "CSEMcp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CSEMcp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CSEMcp.dll"]