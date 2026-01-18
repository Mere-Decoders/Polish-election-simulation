# ---------------------------
# Base runtime image
# ---------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ---------------------------
# Build backend
# ---------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-backend
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy backend project files and restore
COPY ["backend/backend.sln", "backend/"]
COPY ["backend/backend/backend.csproj", "backend/"]
RUN dotnet restore "./backend/backend.csproj"

# Copy backend source
COPY ./backend/backend ./

# Publish backend (skip explicit build)
RUN dotnet publish "./backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---------------------------
# Build frontend
# ---------------------------
FROM node:20 AS build-frontend
WORKDIR /frontend
COPY ./frontend/Polish-election-simulation ./Polish-election-simulation
WORKDIR /frontend/Polish-election-simulation
RUN npm install
RUN npm run build

# ---------------------------
# Final stage
# ---------------------------
FROM base AS final
WORKDIR /app

# Copy backend publish output
COPY --from=build-backend /app/publish ./backend

# Copy frontend build output into wwwroot for static serving
COPY --from=build-frontend /frontend/Polish-election-simulation/dist ./wwwroot

# Run backend
ENTRYPOINT ["dotnet", "backend/backend.dll"]

