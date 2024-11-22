using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Models;
using SurveySystem.API.Services.InterfaceServices;

namespace SurveySystem.API.Services;

public class OptionService(SurveyDbContext context) : IOptionService
{
    public async Task<IEnumerable<Option>> GetOptionsByQuestionIdAsync(Guid questionId)
    {
        return await context.Options
            .Where(o => o.QuestionId == questionId)
            .ToListAsync();
    }

    public async Task<Option?> GetOptionByIdAsync(Guid id)
    {
        return await context.Options.FindAsync(id);
    }

    public async Task<Option> CreateOptionAsync(Option option)
    {
        context.Options.Add(option);
        await context.SaveChangesAsync();
        return option;
    }

    public async Task<Option> UpdateOptionAsync(Option option)
    {
        context.Options.Update(option);
        await context.SaveChangesAsync();
        return option;
    }

    public async Task<bool> DeleteOptionAsync(Guid id)
    {
        var option = await context.Options.FindAsync(id);
        if (option == null) return false;

        context.Options.Remove(option);
        await context.SaveChangesAsync();
        return true;
    }
}