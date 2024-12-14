# Описание API для системы опросов

## Введение
Данный проект представляет собой систему опросов, которая позволяет создавать, обновлять опросы с вопросами и вариантами ответов. Пользователи могут отправлять свои ответы на вопросы, а также просматривать результаты опросов.

## Структура проекта
Проект состоит из нескольких ключевых компонентов:
- **Модели данных**: определяют структуру данных, используемых в системе.
- **Сервисы**: содержат бизнес-логику для работы с данными.
- **DTO (Data Transfer Object)**: объекты, предназначенные для передачи данных между слоями приложения и клиентами.

- ## Технологии

- **Backend**: ASP.NET 
- **База данных**: PostgreSQL
- **ORM**: Entity Framework Core
- **Контейнеризация**: Docker
- **Swagger**: Для тестирования и документации API

- ### Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/get-started) (опционально)

## Модели данных

1. **Survey (Опрос)**
   - `Id`: уникальный идентификатор опроса.
   - `Title`: название опроса.
   - `Description`: описание опроса.
   - `CreatedAt`: дата создания.
   - `Type`: тип опроса (публичный или частный).
   - `Questions`: вопросы, связанные с опросом.

2. **Question (Вопрос)**
   - `Id`: уникальный идентификатор вопроса.
   - `Text`: текст вопроса.
   - `Type`: тип вопроса (например, выбор из нескольких вариантов или текстовый ответ).
   - `SurveyId`: ссылка на опрос, к которому относится вопрос.
   - `Options`: варианты ответов для вопросов с выбором.

3. **Option (Вариант ответа)**
   - `Id`: уникальный идентификатор варианта.
   - `Text`: текст варианта ответа.
   - `QuestionId`: ссылка на вопрос, к которому относится вариант.

4. **Answer (Ответ)**
   - `Id`: уникальный идентификатор ответа.
   - `QuestionId`: идентификатор вопроса, на который был дан ответ.
   - `TextAnswer`: текстовый ответ (для текстовых вопросов).
   - `UserId`: идентификатор пользователя, который дал ответ.
   - `SelectedOptions`: список выбранных вариантов ответов (для вопросов с выбором).

## Описание сервисов

### 1. AnswerService
Сервис для работы с ответами на вопросы.
- **SubmitAnswerAsync**: сохраняет ответ пользователя на вопрос.
- **GetAnswerByIdAsync**: получает ответ по его идентификатору.
- **GetOptionsWithAnswerCountAsync**: возвращает количество ответов на каждый вариант ответа в опросе.

### 2. QuestionService
Сервис для работы с вопросами.
- **UpdateQuestionAsync**: обновляет текст вопроса.
- **UpdateOptionAsync**: обновляет варианты ответа для вопроса.
- **DeleteQuestionAsync**: удаляет вопрос из базы данных.

### 3. SurveyService
Сервис для работы с опросами.
- **CreateSurveyAsync**: создает новый опрос.
- **GetSurveyByIdAsync**: получает информацию об опросе по его идентификатору.
- **UpdateSurveyAsync**: обновляет данные о опросе.

### 4. UserService
Сервис для работы с пользователями.
- **CreateUserAsync**: создает нового пользователя.
- **GetUserByIdAsync**: получает пользователя по его идентификатору.
- **GetAllUsersAsync**: возвращает всех пользователей системы.

## Использование API

Пример создания опроса:
```csharp
var surveyService = new SurveyService(context);
var surveyDto = new SurveyCreateDto
{
    Title = "Новый опрос",
    Description = "Описание опроса",
    Type = SurveyType.PublicType,
    Questions = new List<QuestionCreateDto>
    {
        new QuestionCreateDto
        {
            Text = "Какой ваш любимый язык программирования?",
            Type = QuestionType.SingleChoice,
            Options = new List<OptionCreateDto>
            {
                new OptionCreateDto { Text = "C#" },
                new OptionCreateDto { Text = "Java" },
                new OptionCreateDto { Text = "Python" }
            }
        }
    }
};
var createdSurvey = await surveyService.CreateSurveyAsync(surveyDto);

###Json
{
  "title": "string",
  "description": "string",
  "type": "PublicType",
  "questions": [
    {
      "text": "string",
      "type": "MultipleChoice",
      "options": [
        {
          "text": "string"
        }
      ]
    }
  ]
}
```

Пример отправки ответа:
```csharp
var answerService = new AnswerService(context);
var answer = await answerService.SubmitAnswerAsync(questionId, userId, null, new List<Guid> { optionId });
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

## Заключение

Данная система позволяет эффективно управлять опросами, вопросами и ответами. Каждый компонент API работает асинхронно для обеспечения высокой производительности.
