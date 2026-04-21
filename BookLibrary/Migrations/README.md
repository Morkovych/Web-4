### Установить CLI-инструмент
```
dotnet tool install --global dotnet-ef
```
### Инициализация миграций (на основе моделей)
```
dotnet ef migrations add Initial --project BookLibrary
```
### Создать новую миграцию
```
dotnet ef migrations add <ИмяМиграции> --project BookLibrary
```
### Применить миграцию
```
dotnet ef database update --project BookLibrary
```
### Отменить миграцию
```
dotnet ef migrations remove --project BookLibrary
```
### Пересоздать БД
```
dotnet ef database drop --force --project BookLibrary && dotnet ef database update --project BookLibrary
```
### Сиды
```angular2html
dotnet ef migrations add SeedInitialData --project BookLibrary
dotnet ef database update --project BookLibrary
```