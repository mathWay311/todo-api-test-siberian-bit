REST API для управления списком задач

- **Framework:** .NET 9 (ASP.NET Core)
- **Database:** Entity Framework Core (In-Memory)
- **Validation:** FluentValidation
- **Testing:** xUnit + Moq
- **Infrastructure:** Docker, Swagger


## Запуск проекта

docker-compose up --build

Интерфейс Swagger будет доступен по адресу: http://localhost:5000/swagger

### Через .NET CLI

dotnet run --project TodoApp.Api

### Unit-тесты

dotnet test

