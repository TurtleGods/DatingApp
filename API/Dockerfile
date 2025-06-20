# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
# Copy the project files and restore dependencies
COPY *.csproj ./
# RUN dotnet restore
RUN dotnet restore

# Copy the rest of the application code
COPY . .
RUN dotnet publish -c Release -o /app 

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app
COPY --from=build /app .
EXPOSE 5001
# Set the entry point to your app DLL
ENTRYPOINT ["dotnet", "API.dll"]