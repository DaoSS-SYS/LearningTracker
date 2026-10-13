# LearningTracker

Учебный backend-проект на **C# / ASP.NET Core Web API**: трекер изучаемых тем и учебных сессий.
Написан самостоятельно, без ИИ-агентов — чтобы руками отработать основы backend-разработки.

## Что умеет

- Темы обучения: создание, просмотр, редактирование, удаление, отметка «изучено».
- Учебные сессии по теме: дата начала, длительность в минутах, заметки.
- Статистика по теме: количество сессий и суммарное время.
- Список тем с **поиском**, **фильтром по статусу** и **постраничной выдачей**.
- Удаление темы с сессиями запрещено — приложение возвращает понятную ошибку.

## Стек

| Область | Технологии |
|---|---|
| Платформа | .NET 10, ASP.NET Core Web API |
| Данные | PostgreSQL, Entity Framework Core 10, Npgsql, миграции |
| Документация API | OpenAPI + Swagger UI |
| Тесты | xUnit |

## Архитектура

Многослойная схема **Controller → Service → Repository**:

```
LearningTracker.Api/
├── Controllers/    HTTP-слой: маршруты и коды ответов
├── Services/       бизнес-правила
├── Repositories/   доступ к данным (EF Core), интерфейсы + реализации
├── Dto/            контракты API (запросы, ответы, PagedResult<T>)
├── Filters/        параметры поиска, фильтрации и пагинации
├── Models/         сущности LearningTopic, StudySession
├── Data/           DbContext и миграции
├── Exceptions/     доменные исключения
└── Handlers/       преобразование исключений в ProblemDetails
LearningTracker.Tests/   unit-тесты сервисов (на fake-репозиториях, без БД)
```

Что применено:
- Dependency Injection (scoped-сервисы и репозитории через интерфейсы).
- DTO отдельно от сущностей, валидация через `DataAnnotations`
  (например, `Title` — обязательное, до 50 символов; `PageSize` — от 1 до 100).
- Единый формат ошибок `ProblemDetails` и собственные `IExceptionHandler`
  (`TopicNotFoundException`, `TopicHasSessionsException`).
- Асинхронные методы (`async/await`).

## API

| Метод | Маршрут | Описание |
|---|---|---|
| GET | `/api/LearningTopics?isCompleted=&search=&page=&pageSize=` | список тем (с фильтрами и пагинацией) |
| GET | `/api/LearningTopics/{id}` | тема по id |
| GET | `/api/LearningTopics/{id}/details` | тема с подробностями |
| GET | `/api/LearningTopics/{id}/stats` | статистика по теме |
| POST | `/api/LearningTopics` | создать тему |
| PUT | `/api/LearningTopics/{id}` | обновить тему |
| PATCH | `/api/LearningTopics/{id}/complete` | отметить тему изученной |
| DELETE | `/api/LearningTopics/{id}` | удалить тему (если нет сессий) |
| GET | `/api/LearningTopics/{id}/sessions` | сессии по теме |
| POST | `/api/LearningTopics/{id}/sessions` | добавить сессию |

## Запуск

Нужны: .NET SDK 10 и PostgreSQL.

```bash
cd LearningTracker.Api

# 1. строка подключения — в user-secrets, не в репозитории
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=learningtracker;Username=postgres;Password=<пароль>"

# 2. применить миграции
dotnet ef database update

# 3. запустить
dotnet run
```

В режиме Development доступен Swagger UI с описанием всех методов.

## Тесты

```bash
dotnet test
```

Проверяются правила сервиса тем: ошибка для несуществующей темы, создание темы,
завершение темы, запрет удаления темы с сессиями и удаление темы без сессий.
Также есть тесты для `PagedResult`.
