using SurveySystem.API.DTO;
using SurveySystem.API.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface ISurveyService
{
    public Task<Survey?> CreateSurveyAsync(SurveyCreateDto surveyDto);
    public Task<Survey?> GetSurveyByIdAsync(Guid id);

}