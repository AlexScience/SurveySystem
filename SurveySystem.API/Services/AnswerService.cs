using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services;

public class AnswerService(SurveyDbContext context) : IAnswerService
{
    public async Task<Answer> CreateAnswerAsync(AnswerCreateDto answerDto)
    {
        var answer = new Answer(Guid.NewGuid(), answerDto.QuestionId, answerDto.AnswerText, answerDto.OptionId,
            answerDto.UserId);

        var question = await context.Questions.Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == answerDto.QuestionId);
        if (question == null)
        {
            throw new ArgumentException("Invalid question ID");
        }

        if (answerDto.OptionId.HasValue)
        {
            var option = question.Options.FirstOrDefault(o => o.Id == answerDto.OptionId);
            if (option != null)
            {
                answer.Option = option;
            }
            else
            {
                throw new ArgumentException("Invalid option ID");
            }
        }

        var userExists = await context.Users.AnyAsync(u => u.Id == answerDto.UserId);
        if (userExists)
        {
            answer.User = await context.Users.FirstOrDefaultAsync(a => a.Id == answerDto.UserId);
        }
        else
        {
            throw new ArgumentException("Invalid User ID");
        }

        context.Answers.Add(answer);
        await context.SaveChangesAsync();

        return answer;
    }

    public async Task<Answer> GetAnswerByIdAsync(Guid id)
    {
        return await context.Answers
            .Include(a => a.Question)
            .Include(a => a.Option)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<OptionWithAnswerCountDto>> GetOptionsWithAnswerCountAsync(Guid surveyId)
    {
        var survey = await context.Surveys
            .Include(s => s.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(s => s.Id == surveyId);

        if (survey == null)
            throw new ArgumentException($"Survey with ID {surveyId} not found.");

        var questionIds = survey.Questions.Select(q => q.Id).ToList();

        var optionsWithCounts = await context.Answers
            .Where(a => questionIds.Contains(a.IdQuestion))
            .GroupBy(a => a.OptionId)
            .Select(g => new
            {
                OptionId = g.Key,
                AnswerCount = g.Count()
            })
            .ToListAsync();

        var result = survey.Questions
            .SelectMany(q => q.Options)
            .Select(option => new OptionWithAnswerCountDto
            {
                OptionId = option.Id,
                Text = option.Text,
                AnswerCount = optionsWithCounts.FirstOrDefault(o => o.OptionId == option.Id)?.AnswerCount ?? 0
            })
            .ToList();

        return result;
    }
}