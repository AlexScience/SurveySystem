using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SurveySystem.API.Services;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.API.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Регистрация DbContext с подключением к базе данных
builder.Services.AddDbContext<SurveyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));

// Регистрация сервисов
builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IOptionService, OptionService>();

// Добавление контроллеров и Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations(); // Включение аннотаций
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Survey API",
        Version = "v1",
        Description = "An API for creating and managing surveys",
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Survey API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

/*
 
 {
       "Title": "Survey about Technology",
       "Description": "Survey on the latest trends in technology.",
       "Questions": [
           {
               "Text": "What is your favorite programming language?",
               "Type": "MultipleChoice",
               "Options": [
                   {"Text": "C#"},
                   {"Text": "JavaScript"},
                   {"Text": "Python"}
               ]
           }
       ]
   }
   
*/