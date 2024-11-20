# Базовий образ для .NET
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Встановлення Java (для javac) та Python
RUN apt-get update && \
    apt-get install -y openjdk-11-jdk python3 python3-pip && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

# Копіюємо всі файли проєкту
COPY . ./

# Відновлення залежностей та побудова проєкту
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Перехід до фінального образу
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# Порт для роботи з API (якщо потрібно)
EXPOSE 5000

# Запуск програми
ENTRYPOINT ["dotnet", "HomeworkAssignment.dll"]