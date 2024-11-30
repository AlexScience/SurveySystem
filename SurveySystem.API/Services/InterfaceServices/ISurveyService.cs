using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface ISurveyService
{
    public Task<SurveyWithDetailsDto> CreateSurveyAsync(SurveyCreateDto surveyDto);
    public Task<Survey?> GetSurveyByIdAsync(Guid id);

}