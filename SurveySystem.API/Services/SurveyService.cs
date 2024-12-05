using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services;

public class SurveyService(SurveyDbContext context,IAnswerService answerService) : ISurveyService
{
    public async Task<SurveyWithAnswerCountDto> CreateSurveyAsync(SurveyCreateDto surveyDto)
    {
        // Создание нового опроса
        var survey = new Survey(Guid.NewGuid(), surveyDto.Title, surveyDto.Description, DateTime.UtcNow,surveyDto.Type);

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
        var surveyWithDetails = GetSurveyByIdAsync(survey.Id);

        return await surveyWithDetails;
    }

    public async Task<SurveyWithAnswerCountDto> GetSurveyByIdAsync(Guid id)
    {
        var survey = await context.Surveys.AsNoTracking()
            .Include(survey => survey.Questions).ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(s => s.Id == id);

        var surveyWithAnswerCount = new SurveyWithAnswerCountDto
        {
            Id = survey.Id,
            Title = survey.Title,
            Description = survey.Description,
            CreatedAt = survey.CreatedAt,
            Questions = new List<QuestionWithOptionsDto>()
        };

        foreach (var question in survey.Questions)
        {
            var optionsWithAnswerCount = await answerService.GetOptionsWithAnswerCountAsync(id);
            surveyWithAnswerCount.Questions.Add(new QuestionWithOptionsDto
            {
                QuestionId = question.Id,
                QuestionText = question.Text,
                Options = optionsWithAnswerCount,
            });
        }

        return  surveyWithAnswerCount;
    }

    public async Task<SurveyWithAnswerCountDto> UpdateSurveyAsync(Guid id, SurveyUpdateDto surveyUpdateDto)
    {
        var survey = await context.Surveys.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        survey = survey with { Title = surveyUpdateDto.Title, Description = surveyUpdateDto.Description };

        context.Surveys.Update(survey);
        await context.SaveChangesAsync();
        
        var surveyDto = GetSurveyByIdAsync(id);

        return await surveyDto;
    }
}