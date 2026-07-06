# Giai đoạn 1: Build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Sao chép các file dự án để restore dependencies trước (tối ưu hóa cache)
COPY ["MedicalRecordService.Api/MedicalRecordService.Api.csproj", "MedicalRecordService.Api/"]
COPY ["MedicalRecordService.Data/MedicalRecordService.Data.csproj", "MedicalRecordService.Data/"]
COPY ["MedicalRecordService.Models/MedicalRecordService.Models.csproj", "MedicalRecordService.Models/"]

RUN dotnet restore "MedicalRecordService.Api/MedicalRecordService.Api.csproj"

# Sao chép toàn bộ mã nguồn còn lại và build
COPY . .
WORKDIR "/src/MedicalRecordService.Api"
RUN dotnet build "MedicalRecordService.Api.csproj" -c Release -o /app/build

# Giai đoạn 2: Publish bản phát hành release
FROM build AS publish
RUN dotnet publish "MedicalRecordService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Giai đoạn 3: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Cổng mặc định của .NET 9
EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "MedicalRecordService.Api.dll"]
