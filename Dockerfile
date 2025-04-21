# Use .NET 9.0 SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy and navigate to the right folder
COPY ./WorkshopGrantSystem ./WorkshopGrantSystem
WORKDIR /app/WorkshopGrantSystem

# Restore and publish
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image with ASP.NET Core 9.0
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/WorkshopGrantSystem/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "WorkshopGrantSystem.dll"]
