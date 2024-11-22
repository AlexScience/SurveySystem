using Microsoft.EntityFrameworkCore;
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
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();