using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services;

public class SurveyService(SurveyDbContext context) : ISurveyService
{
    public async Task<SurveyWithAnswerCountDto> CreateSurveyAsync(SurveyCreateDto surveyDto)
    {
        var survey = new Survey(Guid.NewGuid(), surveyDto.Title, surveyDto.Description, DateTime.UtcNow,
            surveyDto.Type);

        foreach (var questionDto in surveyDto.Questions)
        {
            if (string.IsNullOrWhiteSpace(questionDto.Text))
            {
                throw new ArgumentException("Question text cannot be null or empty", nameof(questionDto.Text));
            }

            var questionType = questionDto.Type;
            var question = new Question(Guid.NewGuid(), questionDto.Text, questionType, survey.Id);

            if (questionDto.Options != null && (questionType == QuestionType.MultipleChoice ||
                                                questionType == QuestionType.SingleChoice))
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

        var surveyWithDetails = await GetSurveyByIdAsync(survey.Id);
        return surveyWithDetails;
    }

    public async Task<SurveyWithAnswerCountDto> GetSurveyByIdAsync(Guid id)
    {
        var survey = await context.Surveys.AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new SurveyWithAnswerCountDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                Type = s.Type,
                Questions = s.Questions.Select(q => new QuestionWithOptionsDto
                {
                    QuestionId = q.Id,
                    QuestionText = q.Text,
                    Options = q.Options.Select(o => new OptionWithAnswerCountDto
                    {
                        OptionId = o.Id,
                        Text = o.Text,
                        AnswerCount = q.Answers.Count(a => a.SelectedOptions.Any(opt => opt.Id == o.Id)),
                        AnsweredUsers = s.Type == SurveyType.PublicType
                            ? q.Answers.Where(a => a.SelectedOptions.Any(opt => opt.Id == o.Id))
                                .Select(a => new CreateUserRequestDto
                                {
                                    Id = a.UserId,
                                    Username = a.User != null ? a.User.UserName : "Unknown"
                                }).ToList()
                            : null
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (survey == null)
            throw new KeyNotFoundException("Survey not found");

        return survey;
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