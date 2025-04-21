# Use .NET SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy only the WorkshopGrantSystem subfolder
COPY ./WorkshopGrantSystem ./WorkshopGrantSystem
WORKDIR /app/WorkshopGrantSystem

# Restore and publish
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/WorkshopGrantSystem/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "WorkshopGrantSystem.dll"]
