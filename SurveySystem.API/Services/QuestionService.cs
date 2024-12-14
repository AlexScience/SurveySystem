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
        var question = await context.Questions.FirstOrDefaultAsync(q => q.Id == id);

        if (question == null)
        {
            throw new ArgumentException($"Question with ID {id} not found.");
        }
        
        question = question with { Text = questionUpdateDto.Text };

        await context.SaveChangesAsync();

        return question;
    }
    
    public async Task<Question> UpdateOptionAsync(Guid id, OptionUpdateDto optionUpdateDto)
    {
        var question = await context.Questions.AsNoTracking()
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question == null)
        {
            throw new ArgumentException($"Question with ID {id} not found.");
        }
        
        var updatedQuestion = new Question(question.Id, question.Text, question.Type, question.SurveyId)
        {
            Options = question.Options.ToList(),
            Answers = question.Answers.ToList(),
            Survey = question.Survey
        };

        foreach (var optionUpdate in optionUpdateDto.Options)
        {
            var option = updatedQuestion.Options.FirstOrDefault(o => o.Id == optionUpdate.OptionId);
            if (option != null)
            {
                option = option with { Text = optionUpdate.Text };
            }
            else
            {
                throw new ArgumentException($"Option with ID {optionUpdate.OptionId} not found in question.");
            }
        }

        context.Update(updatedQuestion);
        await context.SaveChangesAsync();
        return updatedQuestion;
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