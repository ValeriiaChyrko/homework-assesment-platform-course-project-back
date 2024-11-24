# Базовий образ для .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Копіюємо файли проектів для відновлення залежностей
COPY HomeworkAssignment.API/HomeworkAssignment.API.csproj HomeworkAssignment.API/
COPY HomeworkAssignment.Application/HomeworkAssignment.Application.csproj HomeworkAssignment.Application/
COPY HomeworkAssignment.DTOs/HomeworkAssignment.DTOs.csproj HomeworkAssignment.DTOs/
COPY HomeworkAssignment.Database/HomeworkAssignment.Database.csproj HomeworkAssignment.Database/
COPY HomeworkAssignment.Domain/HomeworkAssignment.Domain.csproj HomeworkAssignment.Domain/
COPY HomeworkAssignment.Persistence/HomeworkAssignment.Persistence.csproj HomeworkAssignment.Persistence/
COPY HomeworkAssignment.Infrastructure/HomeworkAssignment.Infrastructure.csproj HomeworkAssignment.Infrastructure/

# Відновлення залежностей
RUN dotnet restore "HomeworkAssignment.API/HomeworkAssignment.API.csproj"

# Копіюємо всі файли та будуємо проект
COPY . .
RUN dotnet build "HomeworkAssignment.API/HomeworkAssignment.API.csproj" -c Release -o /app/build

# Публікація
FROM build AS publish
RUN dotnet publish "HomeworkAssignment.API/HomeworkAssignment.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Фінальний образ для запуску
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeworkAssignment.API.dll"]