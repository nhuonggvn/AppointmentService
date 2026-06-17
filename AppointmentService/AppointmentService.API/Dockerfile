# Base stage for running the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 80

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["AppointmentService/AppointmentService.API/AppointmentService.API.csproj", "AppointmentService/AppointmentService.API/"]
COPY ["AppointmentService/AppointmentService.Domain/AppointmentService.Domain.csproj", "AppointmentService/AppointmentService.Domain/"]
COPY ["AppointmentService/AppointmentService.Infrastructure/AppointmentService.Infrastructure.csproj", "AppointmentService/AppointmentService.Infrastructure/"]

RUN dotnet restore "AppointmentService/AppointmentService.API/AppointmentService.API.csproj"

# Copy all source files and build
COPY . .
WORKDIR "/src/AppointmentService/AppointmentService.API"
RUN dotnet build "AppointmentService.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "AppointmentService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppointmentService.API.dll"]
