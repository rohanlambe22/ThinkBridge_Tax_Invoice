# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore \
 && dotnet publish -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published app
COPY --from=build /app/out .

# Expose port (Render injects $PORT)
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

# Ensure init.sql is present in working dir
COPY init.sql ./

ENTRYPOINT ["dotnet", "display_an_invoice (2).dll"]

