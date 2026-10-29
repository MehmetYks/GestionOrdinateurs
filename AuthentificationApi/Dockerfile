# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copie des csproj et restore
COPY ./AuthentificationApi/AuthentificationApi.csproj ./AuthentificationApi/
COPY ./GestionOrdinateurs.Shared/GestionOrdinateurs.Shared.csproj ./GestionOrdinateurs.Shared/

RUN dotnet restore ./AuthentificationApi/AuthentificationApi.csproj

# Copie du code
COPY ./AuthentificationApi/ ./AuthentificationApi/
COPY ./GestionOrdinateurs.Shared/ ./GestionOrdinateurs.Shared/

RUN dotnet publish ./AuthentificationApi/AuthentificationApi.csproj -c Release -o /app/out

# =========================
# Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/out ./

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5157

EXPOSE 5157

ENTRYPOINT ["dotnet", "AuthentificationApi.dll"]