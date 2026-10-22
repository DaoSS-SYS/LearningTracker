# LearningTracker

[![CI](https://github.com/DaoSS-SYS/LearningTracker/actions/workflows/ci.yml/badge.svg)](https://github.com/DaoSS-SYS/LearningTracker/actions/workflows/ci.yml) [![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Учебный backend-проект на **C# / ASP.NET Core Web API**: трекер изучаемых тем и учебных сессий.
Написан самостоятельно, без ИИ-агентов — чтобы руками отработать основы backend-разработки.

## Что умеет

- **Регистрация и вход** по email и паролю. После входа выдаётся **JWT-токен**; все остальные методы доступны только с ним.
- **Роли:** `User` (по умолчанию) и `Admin`. Некоторые операции доступны только администратору.
- Темы обучения: создание, просмотр, редактирование, удаление, отметка «изучено».
- Учебные сессии по теме: дата начала, длительность в минутах, заметки.
- Статистика по теме: количество сессий и суммарное время.
- Список тем с **поиском**, **фильтром по статусу** и **постраничной выдачей**.
- **Категории:** справочник, который читают все авторизованные пользователи, а создаёт, меняет и удаляет администратор. Название категории уникально без учёта регистра, категорию с темами удалить нельзя.
- Удаление темы с сессиями запрещено — приложение возвращает понятную ошибку.

> Связь «тема — категория» уже есть в модели данных (`LearningTopic.CategoryId`), но назначать категорию теме через API пока нельзя.

## Стек

| Область | Технологии |
|---|---|
| Платформа | .NET 10, ASP.NET Core Web API |
| Данные | PostgreSQL, Entity Framework Core 10, Npgsql, миграции |
| Безопасность | JWT Bearer, хеширование паролей `PasswordHasher` (ASP.NET Core Identity), авторизация по ролям |
| Логирование | Serilog (консоль и файлы по дням) |
| Документация API | OpenAPI + Swagger UI |
| Тесты | xUnit |
| CI | GitHub Actions (сборка и тесты) |

## Архитектура

Многослойная схема **Controller → Service → Repository**:

```
LearningTracker.Api/
├── Controllers/    HTTP-слой: маршруты, коды ответов, [Authorize]
├── Services/       бизнес-правила, AuthService, JwtTokenService
├── Repositories/   доступ к данным (EF Core), интерфейсы + реализации
├── Dto/            контракты API (запросы, ответы, PagedResult<T>)
├── Filters/        параметры поиска, фильтрации и пагинации
├── Mapping/        преобразование сущностей в DTO
├── Models/         сущности LearningTopic, StudySession, User, Category
├── Data/           DbContext и миграции
├── Exceptions/     доменные исключения
└── Handlers/       преобразование исключений в ProblemDetails
LearningTracker.Tests/   unit-тесты сервисов (на fake-репозиториях, без БД)
```

Что применено:
- Dependency Injection (scoped-сервисы и репозитории через интерфейсы).
- DTO отдельно от сущностей, валидация через `DataAnnotations`
  (например, `Title` — обязательное, до 50 символов; пароль — не короче 8 символов; `PageSize` — от 1 до 100).
- **JWT-аутентификация:** токен подписывается HMAC-SHA256, в нём идентификатор пользователя, email и роль, срок жизни задаётся настройкой `Jwt:ExpiresMinutes` (по умолчанию 15 минут). Издатель, аудитория и ключ проверяются при каждом запросе.
- Пароли хранятся только в виде хеша, email нормализуется (нижний регистр, без пробелов).
- Единый формат ошибок `ProblemDetails` и собственные `IExceptionHandler`
  (тема, категория, пользователь, неверные учётные данные и др.).
- Асинхронные методы (`async/await`).

## API

Доступ: **открыт** — без токена, **любой** — любой авторизованный пользователь, **Admin** — только администратор.

| Метод | Маршрут | Доступ | Описание |
|---|---|---|---|
| POST | `/api/Auth/register` | открыт | регистрация (роль `User`) |
| POST | `/api/Auth/login` | открыт | вход, в ответе JWT-токен |
| GET | `/api/LearningTopics?isCompleted=&search=&page=&pageSize=` | любой | список тем (с фильтрами и пагинацией) |
| GET | `/api/LearningTopics/{id}` | любой | тема по id |
| GET | `/api/LearningTopics/{id}/details` | любой | тема с подробностями |
| GET | `/api/LearningTopics/{id}/stats` | любой | статистика по теме |
| POST | `/api/LearningTopics` | любой | создать тему |
| PUT | `/api/LearningTopics/{id}` | любой | обновить тему |
| PATCH | `/api/LearningTopics/{id}/complete` | любой | отметить тему изученной |
| DELETE | `/api/LearningTopics/{id}` | Admin | удалить тему (если нет сессий) |
| GET | `/api/LearningTopics/{id}/sessions` | любой | сессии по теме |
| POST | `/api/LearningTopics/{id}/sessions` | любой | добавить сессию |
| GET | `/api/Categories`, `/api/Categories/{id}` | любой | список категорий, категория по id |
| POST | `/api/Categories` | Admin | создать категорию |
| PUT | `/api/Categories/{id}` | Admin | переименовать категорию |
| DELETE | `/api/Categories/{id}` | Admin | удалить категорию (если в ней нет тем) |

## Запуск

Нужны: .NET SDK 10 и PostgreSQL.

```bash
cd LearningTracker.Api

# 1. секреты — в user-secrets, не в репозитории
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=learningtracker;Username=postgres;Password=<пароль>"
dotnet user-secrets set "Jwt:Key" "<длинная случайная строка, не короче 32 символов>"

# 2. применить миграции
dotnet ef database update

# 3. запустить
dotnet run
```

В режиме Development доступен Swagger UI с описанием всех методов.

Пример работы с API:

```bash
# регистрация и вход
curl -X POST https://localhost:<порт>/api/Auth/register -H "Content-Type: application/json" -d '{"email":"user@example.com","password":"password123"}'
curl -X POST https://localhost:<порт>/api/Auth/login    -H "Content-Type: application/json" -d '{"email":"user@example.com","password":"password123"}'

# запрос с токеном из ответа на login
curl https://localhost:<порт>/api/LearningTopics -H "Authorization: Bearer <токен>"
```

Все новые пользователи получают роль `User`. Роль `Admin` назначается вручную в базе данных.

## Тесты

```bash
dotnet test
```

23 unit-теста на fake-репозиториях, без базы данных:
- **аутентификация:** регистрация (занятый и свободный email), вход (неверный email, неверный пароль, успешный вход);
- **JWT:** токен из трёх частей, claims пользователя, ошибка при отсутствии ключа, срок жизни токена;
- **темы:** создание, обновление, завершение, удаление (запрет при наличии сессий), «не найдено»;
- **сессии:** создание и получение, «не найдено»;
- `PagedResult` (расчёт числа страниц).

Тесты запускаются в GitHub Actions при каждом push и pull request.

## Лицензия

MIT, см. [LICENSE](LICENSE).
