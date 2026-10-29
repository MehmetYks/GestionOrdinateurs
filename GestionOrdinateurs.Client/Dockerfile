# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY ./GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj ./GestionOrdinateurs.Client/

RUN dotnet restore ./GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj

COPY ./GestionOrdinateurs.Client/ ./GestionOrdinateurs.Client/

RUN dotnet publish ./GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj -c Release -o /app/out

# =========================
# Runtime nginx
# =========================
FROM nginx:alpine AS runtime

COPY ./GestionOrdinateurs.Client/nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=build /app/out/wwwroot /usr/share/nginx/html

EXPOSE 5000