using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services.InterfaceServices;

namespace SurveySystem.API.Services;

public class CounterService(SurveyDbContext context) : ICounterService
{
    public async Task<int> CountUniqueUsersAsync(Guid surveyId)
    {
        var uniqueUserIds = await context.Answers
            .Where(a => a.Question.SurveyId == surveyId) // Фильтрация по SurveyId
            .Select(a => a.UserId) // Извлечение UserId
            .Distinct() // Уникальные UserId
            .ToListAsync();

        return uniqueUserIds.Count;
    }
}