# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files for restore
COPY ["wallet_and_loans/wallet_and_loans_api.csproj", "wallet_and_loans/"]
COPY ["wallet_and_loans_components/wallet_and_loans_components.csproj", "wallet_and_loans_components/"]

# Restore dependencies for the main API project
RUN dotnet restore "wallet_and_loans/wallet_and_loans_api.csproj"

# Copy all source files
COPY wallet_and_loans/ ./wallet_and_loans/
COPY wallet_and_loans_components/ ./wallet_and_loans_components/

# Build and publish the application
WORKDIR "/src/wallet_and_loans"
RUN dotnet publish \
    "wallet_and_loans_api.csproj" \
    -c "$BUILD_CONFIGURATION" \
    -o /app/publish \
    /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "wallet_and_loans_api.dll"]
