using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.DTO;
using SurveySystem.API.Models;
using SurveySystem.API.Services.InterfaceServices;

namespace SurveySystem.API.Services;

public class SurveyService(SurveyDbContext context) : ISurveyService
{
    
    public async Task<Survey> CreateSurveyAsync(SurveyCreateDto surveyDto)
    {
        var survey = new Survey(Guid.NewGuid(), surveyDto.Title, surveyDto.Description, DateTime.UtcNow);

        foreach (var questionDto in surveyDto.Questions)
        {
            var question = new Question(Guid.NewGuid(), questionDto.Text, questionDto.Type, survey.Id);

            foreach (var optionDto in questionDto.Options)
            {
                var option = new Option(Guid.NewGuid(), optionDto.Text, question.Id);
                question.Options.Add(option);
            }

            survey.Questions.Add(question);
        }

        await context.Surveys.AddAsync(survey);
        await context.SaveChangesAsync();

        return survey;
    }

    public async Task<Survey> GetSurveyByIdAsync(Guid id)
    {
        return await context.Surveys
            .Include(s => s.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}