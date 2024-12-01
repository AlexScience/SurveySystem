using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services;

public class QuestionService(SurveyDbContext context) : IQuestionService
{
    public async Task<Question> UpdateQuestionAsync(Guid id, QuestionUpdateDto questionUpdateDto)
    {
        var question = await context.Questions.AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
        question = question with { Text = questionUpdateDto.Text };
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