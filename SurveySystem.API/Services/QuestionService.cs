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

    public async Task<Question> UpdateOptionAsync(Guid id, OptionUpdateDto optionCreateDto)
    {
        var question = await context.Questions.Include(q => q.Options).AsNoTracking() 
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question == null)
        {
            throw new ArgumentException($"Question with ID {id} not found.");
        }

        // Обновляем коллекцию Options, создавая новые объекты с изменённым Text
        question = question with
        {
            Options = question.Options.Select(option => option with
            {
                Text = optionCreateDto.Text
            }).ToList()
        };

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