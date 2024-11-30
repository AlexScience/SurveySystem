using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services;

public class SurveyService(SurveyDbContext context) : ISurveyService
{
    public async Task<SurveyWithDetailsDto> CreateSurveyAsync(SurveyCreateDto surveyDto)
    {
        if (surveyDto == null)
            throw new ArgumentNullException(nameof(surveyDto));

        if (surveyDto.Questions == null || !surveyDto.Questions.Any())
            throw new ArgumentException("Survey must contain at least one question", nameof(surveyDto.Questions));

        // Создание нового опроса
        var survey = new Survey(Guid.NewGuid(), surveyDto.Title, surveyDto.Description, DateTime.UtcNow);

        foreach (var questionDto in surveyDto.Questions)
        {
            if (string.IsNullOrWhiteSpace(questionDto.Text))
            {
                throw new ArgumentException("Question text cannot be null or empty", nameof(questionDto.Text));
            }

            // Создание вопроса
            var questionType = questionDto.Type; // Тип вопроса напрямую передаем
            var question = new Question(Guid.NewGuid(), questionDto.Text, questionType, survey.Id);

            // Если тип вопроса — MultipleChoice, то добавляем опции
            if (questionDto.Options != null && questionType == QuestionType.MultipleChoice)
            {
                foreach (var optionDto in questionDto.Options)
                {
                    if (string.IsNullOrWhiteSpace(optionDto.Text))
                    {
                        throw new ArgumentException("Option text cannot be null or empty", nameof(optionDto.Text));
                    }

                    var option = new Option(Guid.NewGuid(), optionDto.Text, question.Id);
                    question.Options.Add(option);
                }
            }

            survey.Questions.Add(question);
        }

        await context.Surveys.AddAsync(survey);
        await context.SaveChangesAsync();

        // Формируем DTO для ответа
        var surveyWithDetails = new SurveyWithDetailsDto
        {
            Id = survey.Id,
            Title = survey.Title,
            Description = survey.Description,
            CreatedAt = survey.CreatedAt,
            Questions = context.Questions
                .Where(q => q.SurveyId == survey.Id)
                .AsEnumerable()
                .Select(q => new QuestionWithOptionsDto
                {
                    QuestionId = q.Id,
                    QuestionText = q.Text,
                    Options = q.Options.Select(o => new OptionWithAnswerCountDto
                    {
                        OptionId = o.Id,
                        Text = o.Text,
                    }).ToList()
                }).ToList()
        };

        return surveyWithDetails;
    }

    public async Task<Survey?> GetSurveyByIdAsync(Guid id)
    {
        return await context.Surveys
            .Include(s => s.Questions)
            .ThenInclude(q => q.Options)
            .Include(s => s.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}