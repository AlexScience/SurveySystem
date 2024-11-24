# Система Опросов

Система позволяет создавать опросы, отвечать на вопросы и просматривать результаты. Этот проект включает API для создания опросов, добавления ответов и получения статистики по ответам.

![{F1ED1F38-5525-47D4-8434-45BC355E4D73}](https://github.com/user-attachments/assets/727b20bf-9e6d-4af7-9a3a-ef0c7fcccd7c)

## Описание

Приложение состоит из нескольких контроллеров, отвечающих за управление пользователями, опросами и ответами. Пользователи могут создать опросы с вопросами и вариантами ответов, а также отправлять свои ответы на вопросы.

## Возможности

- **Создание пользователя** — позволяет создать нового пользователя с уникальными данными.
- **Создание опроса** — возможность создать новый опрос с вопросами и вариантами ответов.
- **Отправка ответов** — пользователи могут отвечать на вопросы опросов.
- **Получение результатов** — просмотр статистики по ответам на вопросы.

## Технологии

- **Backend**: ASP.NET Core 
- **База данных**: PostgreSQL
- **ORM**: Entity Framework Core
- **Контейнеризация**: Docker
- **Swagger**: Для тестирования и документации API

## Установка и запуск

### Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/get-started) (опционально)

### Клонирование репозитория

```bash
git clone https://github.com/ваш_репозиторий.git
cd имя_проекта
# Проект для опросов с API
```

## Клонирование репозитория

Чтобы начать работу с проектом, выполните следующие шаги:

```bash
git clone https://github.com/ваш_репозиторий.git
cd имя_проекта
```

## Настройка базы данных PostgreSQL

1. Запустите контейнер с PostgreSQL с помощью Docker:

```bash
docker run --name survey-db -e POSTGRES_PASSWORD=qwe123 -p 5432:5432 -d postgres
```

2. Обновите строку подключения в файле `appsettings.json`:

```json
"ConnectionStrings": {
  "postgres": "Server=127.0.0.1;Port=5432;Database=postgres;User Id=survey;Password=qwe123; Include Error Detail=true"
}
```

## Запуск приложения

1. Построить и запустить проект:

```bash
dotnet build
dotnet run
```
## Эндпоинты API

### 1. Пользователи

![{8E76E6BA-909C-4644-B035-7790AB8C44C8}](https://github.com/user-attachments/assets/ad391cb1-a8c5-4916-b9c5-bbc2ba178496)
#### Создание пользователя
`POST /api/user/create`  
Создаёт нового пользователя. В теле запроса передаются: `Username`, `Email`, `Password`, `FullName`.

#### Получить пользователя по ID
`GET /api/user/{id}`  
Получает информацию о пользователе по его ID.

#### Получить всех пользователей
`GET /api/user/all`  
Получает список всех пользователей.

### 2. Опросы
![{6A72B461-E14B-46E5-8519-E953CBA61028}](https://github.com/user-attachments/assets/7a2f71da-ae67-4501-8bb7-9ebdeebbed6b)

#### Создание опроса
`POST /api/surveys`  
Создаёт новый опрос с вопросами и вариантами ответов. В теле запроса передаются: `Title`, `Description`, `Questions`.

#### Получить опрос по ID
`GET /api/surveys/{id}`  
Получает информацию об опросе по его ID, включая статистику по вариантам ответов.

### 3. Ответы
![{CD626A27-D86E-48D2-B66E-955676037AB6}](https://github.com/user-attachments/assets/323121ef-66e9-405f-aada-c4dd76445797)

#### Отправить ответ на вопрос
`POST /api/answers`  
Отправляет ответ на вопрос. В теле запроса передаются: `QuestionId`, `OptionId`, `UserId`.

#### Получить ответ по ID
`GET /api/answers/{id}`  
Получает информацию об ответе по его ID.

#### Получить варианты ответов с количеством ответов для вопроса
`GET /api/answers/options/{questionId}`  
Получает варианты ответов и количество ответов для каждого варианта для конкретного вопроса.

## Пример запросов и ответов

### Пример создания опроса:

```json
{
  "title": "Опрос о любимом языке программирования",
  "description": "Определите свой любимый язык программирования.",
  "questions": [
    {
      "text": "Какой ваш любимый язык программирования?",
      "type": 0,
      "options": [
        { "text": "C#" },
        { "text": "Java" },
        { "text": "Python" },
        { "text": "JavaScript" }
      ]
    }
  ]
}
```

### Пример ответа на вопрос:

```json
{
  "questionId": "e2806bdb-ca7c-4044-a279-56c5f40be6cb",
  "optionId": "680961ab-3dd8-46ac-8c97-b7b765079bff",
  "userId": "12345678-90ab-cdef-1234-567890abcdef"
}
```

### Пример ответа на запрос статистики по вариантам:

```json
{
  "questionId": "e2806bdb-ca7c-4044-a279-56c5f40be6cb",
  "options": [
    {
      "optionId": "680961ab-3dd8-46ac-8c97-b7b765079bff",
      "text": "C#",
      "answerCount": 10
    },
    {
      "optionId": "9d4207cb-94c0-4b23-b992-fc3241a8330d",
      "text": "Java",
      "answerCount": 5
    },
    {
      "optionId": "c1c4461e-5a1f-45f3-bc09-6ac75898f971",
      "text": "Python",
      "answerCount": 3
    }
  ]
}
```
