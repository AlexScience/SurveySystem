using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.DTO;
using SurveySystem.API.Models;
using SurveySystem.API.Services.InterfaceServices;

namespace SurveySystem.API.Services;

public class AnswerService(SurveyDbContext context) : IAnswerService
{
    public async Task<Answer> CreateAnswerAsync(AnswerCreateDto answerDto)
    {
        var answer = new Answer(Guid.NewGuid(), answerDto.QuestionId, answerDto.AnswerText, answerDto.OptionId, answerDto.UserId);

        var question = await context.Questions.Include(q => q.Options).FirstOrDefaultAsync(q => q.Id == answerDto.QuestionId);
        if (question == null)
            throw new ArgumentException("Invalid question ID");

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
}
