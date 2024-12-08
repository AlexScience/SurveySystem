using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services;

public class SurveyService(SurveyDbContext context, IAnswerService answerService) : ISurveyService
{
    public async Task<SurveyWithAnswerCountDto> CreateSurveyAsync(SurveyCreateDto surveyDto)
    {
        // Создание нового опроса
        var survey = new Survey(Guid.NewGuid(), surveyDto.Title, surveyDto.Description, DateTime.UtcNow,
            surveyDto.Type);

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
            .Include(s => s.Questions)
            .ThenInclude(q => q.Options)
            .Include(s => s.Questions)
            .ThenInclude(q => q.Answers).ThenInclude(answer => answer.User)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (survey == null)
        {
            throw new KeyNotFoundException("Survey not found");
        }

        var surveyWithAnswerCount = new SurveyWithAnswerCountDto
        {
            Title = survey.Title,
            Description = survey.Description,
            CreatedAt = survey.CreatedAt,
            Type = survey.Type,
            Questions = new List<QuestionWithOptionsDto>()
        };

        foreach (var question in survey.Questions)
        {
            var optionsWithAnswerCount = new List<OptionWithAnswerCountDto>();

            foreach (var option in question.Options)
            {
                // Список ответов для данной опции
                var answers = question.Answers.Where(a => a.OptionId == option.Id).ToList();

                // Если опрос анонимный, не включаем список пользователей
                List<CreateUserRequestDto>? answeredUsers = null;

                if (survey.Type == SurveyType.PublicType)
                {
                    answeredUsers = answers
                        .Select(a => new CreateUserRequestDto { Id = a.UserId, Username = a.User.UserName })
                        .ToList();
                }

                optionsWithAnswerCount.Add(new OptionWithAnswerCountDto
                {
                    OptionId = option.Id,
                    Text = option.Text,
                    AnswerCount = answers.Count,
                    AnsweredUsers = answeredUsers // null для анонимных опросов
                });
            }

            surveyWithAnswerCount.Questions.Add(new QuestionWithOptionsDto
            {
                QuestionId = question.Id,
                QuestionText = question.Text,
                Options = optionsWithAnswerCount
            });
        }

        return surveyWithAnswerCount;
    }


    public async Task<SurveyWithAnswerCountDto> UpdateSurveyAsync(Guid id, SurveyUpdateDto surveyUpdateDto)
    {
        var survey = await context.Surveys.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        survey = survey with
        {
            Title = surveyUpdateDto.Title, Description = surveyUpdateDto.Description, Type = surveyUpdateDto.Type
        };

        context.Surveys.Update(survey);
        await context.SaveChangesAsync();

        var surveyDto = GetSurveyByIdAsync(id);

        return await surveyDto;
    }
}