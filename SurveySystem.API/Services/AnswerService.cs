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

    public async Task<List<OptionWithAnswerCountDto>> GetOptionsWithAnswerCountAsync(Guid questionId)
    {
        var options = await context.Options
            .Where(o => o.QuestionId == questionId)
            .ToListAsync();

        var result = new List<OptionWithAnswerCountDto>();
        foreach (var option in options)
        {
            var answerCount = await context.Answers
                .CountAsync(a => a.OptionId == option.Id);

            result.Add(new OptionWithAnswerCountDto
            {
                OptionId = option.Id,
                Text = option.Text,
                AnswerCount = answerCount
            });
        }

        return result;
    }
}