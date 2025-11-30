# 📚 Блок 4. Веб-разработка на .NET - REST API для управления библиотекой книг

## 🎯 Общее описание
Разработайте REST API для управления электронной библиотекой книг с использованием ASP.NET Core и современных практик веб-разработки.

**Стек:** ASP.NET Core, Swagger, FluentValidation, xUnit

---

## Важно!
Так как следующее задание будет выполняться в этом же репозитории, делайте MR в **свой** репозиторий, после проверки слейте его.

## 📋 Задачи

### Задача 1: Базовая настройка проекта

- Настроить Swagger/OpenAPI
- Добавить логирование с использованием `ILogger` во все методы API
- Создать модель `Book` со свойствами:
    - `Id` (int, первичный ключ)
    - `Title` (string, обязательное)
    - `Author` (string, обязательное)
    - `ISBN` (string, уникальный)
    - `PublicationYear` (int, диапазон 1000-текущий год)
    - `Genre` (string)
    - `IsAvailable` (bool)

### Задача 2: Минимальные API и CRUD операции

Реализовать следующие endpoints с помощью контроллеров:

```
GET    /api/books          - получить все книги
GET    /api/books/{id}     - получить книгу по ID
POST   /api/books          - добавить новую книгу
PUT    /api/books/{id}     - обновить книгу
DELETE /api/books/{id}     - удалить книгу
```

**Требования:**
- Использовать статический список книг для хранения данных
- Реализовать базовую обработку ошибок (404 для ненайденной книги)

### Задача 3: Валидация моделей

- Добавить валидацию с помощью **DataAnnotations** в модель `Book`
- Реализовать кастомную валидацию для `ISBN` (формат: XXX-XX-XXXX-XXX-X)
- Создать **FluentValidation** валидатор для модели `Book`

### Задача 4: Фильтрация и сортировка

Расширить endpoint `GET /api/books`:

**Параметры запроса:**
- `author` - фильтрация по автору (частичное совпадение)
- `sortBy` - сортировка по `title`

**Пример:**
```
GET /api/books?author=Tolkien&sortBy=title
```

### Задача 5: Middleware и фильтры

- Создать фильтр исключений для обработки всех необработанных ошибок
- Добавить middleware для обработки несуществующих routes (возвращать стандартизированный JSON)

**Формат ответа для несуществующих routes:**
```json
{
  "success": false,
  "message": "Route not found",
  "path": "/api/nonexistent",
  "timestamp": "2024-01-15T10:30:00Z"
}
```

### Задача 6: Форматы ответа и документация

- Реализовать стандартизированный формат ответа для всех endpoints:
```json
{
  "success": true,
  "data": { ... },
  "message": "Operation completed successfully"
}
```

### Задача 7: Тестирование

- Написать **unit-тесты** для сервисов
- Написать **интеграционные тесты** для контроллеров/minimal APIs

---

## 📝 Пример данных для тестирования

```csharp
// Стартовый набор книг для тестирования
private static List<Book> _books = new()
{
    new Book { Id = 1, Title = "The Lord of the Rings", Author = "J.R.R. Tolkien", ISBN = "978-0544003415", PublicationYear = 1954, Genre = "Fantasy", IsAvailable = true },
    new Book { Id = 2, Title = "1984", Author = "George Orwell", ISBN = "978-0451524935", PublicationYear = 1949, Genre = "Dystopian", IsAvailable = true },
    new Book { Id = 3, Title = "Pride and Prejudice", Author = "Jane Austen", ISBN = "978-0141439518", PublicationYear = 1813, Genre = "Romance", IsAvailable = false }
};
```

---

## 📞 Полезные ссылки

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [FluentValidation Documentation](https://docs.fluentvalidation.net/)
- [Swagger/OpenAPI](https://swagger.io/)
- [xUnit Testing](https://xunit.net/)

**Удачи в разработке! 🚀**
