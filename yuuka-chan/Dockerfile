# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project file
COPY ["yuuka-chan.csproj", "./"]

# Restore dependencies
RUN dotnet restore "yuuka-chan.csproj"

# Copy all source files
COPY . .

# Build and publish the application
RUN dotnet publish \
    "yuuka-chan.csproj" \
    -c "$BUILD_CONFIGURATION" \
    -o /app/publish \
    /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .


ENTRYPOINT ["dotnet", "yuuka-chan.dll"]
