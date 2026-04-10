# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY DeviceManagementSystem.slnx ./
COPY DeviceManagementSystem/*.csproj DeviceManagementSystem/
COPY DeviceManagementSystem.Application/*.csproj DeviceManagementSystem.Application/
COPY DeviceManagementSystem.Domain/*.csproj DeviceManagementSystem.Domain/
COPY DeviceManagementSystem.Infrastructure/*.csproj DeviceManagementSystem.Infrastructure/
RUN dotnet restore

COPY DeviceManagementSystem/. DeviceManagementSystem/
COPY DeviceManagementSystem.Application/. DeviceManagementSystem.Application/
COPY DeviceManagementSystem.Domain/. DeviceManagementSystem.Domain/
COPY DeviceManagementSystem.Infrastructure/. DeviceManagementSystem.Infrastructure/
WORKDIR /src/DeviceManagementSystem
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY DeviceManagementSystem/appsettings.json .

EXPOSE 8080

ENTRYPOINT ["dotnet", "DeviceManagementSystem.dll"]
