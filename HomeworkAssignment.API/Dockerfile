# Базовий образ для .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Встановлення Java та Python (якщо вони потрібні для вашого проєкту)
RUN apt-get update && \
    apt-get install -y openjdk-11-jdk python3 python3-pip && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

# Копіювання проєкту
COPY ["HomeworkAssignment.API/HomeworkAssignment.API.csproj", "HomeworkAssignment.API/"]
COPY ["HomeworkAssignment.Application/HomeworkAssignment.Application.csproj", "HomeworkAssignment.Application/"]
COPY ["HomeworkAssignment.DTOs/HomeworkAssignment.DTOs.csproj", "HomeworkAssignment.DTOs/"]
COPY ["HomeworkAssignment.Database/HomeworkAssignment.Database.csproj", "HomeworkAssignment.Database/"]
COPY ["HomeworkAssignment.Domain/HomeworkAssignment.Domain.csproj", "HomeworkAssignment.Domain/"]
COPY ["HomeworkAssignment.Persistence/HomeworkAssignment.Persistence.csproj", "HomeworkAssignment.Persistence/"]
COPY ["HomeworkAssignment.Infrastructure/HomeworkAssignment.Infrastructure.csproj", "HomeworkAssignment.Infrastructure/"]

# Відновлення залежностей
RUN dotnet restore "HomeworkAssignment.API/HomeworkAssignment.API.csproj"

# Копіювання всіх файлів у контейнер
COPY . .

# Побудова проєкту
WORKDIR "/src/HomeworkAssignment.API"
RUN dotnet build "HomeworkAssignment.API.csproj" -c Release -o /app/build

# Публікація проєкту
FROM build AS publish
RUN dotnet publish "HomeworkAssignment.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Остаточний етап, де налаштовується виконання
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeworkAssignment.API.dll"]
