using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services;

public class QuestionService(SurveyDbContext context) : IQuestionService
{
    public async Task<IEnumerable<Question>> GetQuestionsBySurveyIdAsync(Guid surveyId)
    {
        return await context.Questions
            .Where(q => q.SurveyId == surveyId)
            .Include(q => q.Options)
            .ToListAsync();
    }

    public async Task<Question?> GetQuestionByIdAsync(Guid id)
    {
        return await context.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task<Question> CreateQuestionAsync(Question question)
    {
        context.Questions.Add(question);
        await context.SaveChangesAsync();
        return question;
    }

    public async Task<Question> UpdateQuestionAsync(Question question)
    {
        context.Questions.Update(question);
        await context.SaveChangesAsync();
        return question;
    }

    public async Task<bool> DeleteQuestionAsync(Guid id)
    {
        var question = await context.Questions.FindAsync(id);
        if (question == null) return false;

        context.Questions.Remove(question);
        await context.SaveChangesAsync();
        return true;
    }
}