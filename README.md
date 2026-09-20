# TechStore — ИС управления заказами интернет-магазина

Лабораторная работа №1 по дисциплине «Разработка кода информационных систем».

## Архитектура

| Проект | Слой | Назначение |
|---|---|---|
| `TechStore.Domain` | Domain | Сущности и перечисления, общие для всех слоёв |
| `TechStore.DAL` | Data Access Layer | `AppDbContext` (EF Core), репозитории, Unit of Work |
| `TechStore.BLL` | Business Logic Layer | Сервисы, бизнес-правила, валидация |
| `TechStore.API` | Presentation Layer | REST-контроллеры, DTO, настройка DI |

Направление зависимостей: `API -> BLL -> DAL -> Domain`. Обратных ссылок нет.

## Запуск

```bash
dotnet restore
dotnet ef migrations add Initial --project TechStore.DAL --startup-project TechStore.API
dotnet ef database update --project TechStore.DAL --startup-project TechStore.API
dotnet run --project TechStore.API
```

Swagger UI: `https://localhost:7xxx/swagger`

## Пример запроса

```http
POST /api/orders
Content-Type: application/json

{
  "customerId": 1,
  "addressId": 1,
  "items": [ { "productId": 5, "quantity": 2 } ]
}
```

---

## Статус сборки

![build](https://github.com/Pixtohub10167/TechStore/actions/workflows/build.yml/badge.svg)
