namespace SurveySystem.API.Services.InterfaceServices;

public interface ICounterService
{
    public Task<int> CountUniqueUsersAsync(Guid surveyId);
}