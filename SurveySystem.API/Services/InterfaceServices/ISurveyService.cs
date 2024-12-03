using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface ISurveyService
{
    public Task<SurveyWithAnswerCountDto> CreateSurveyAsync(SurveyCreateDto surveyDto);
    public Task<SurveyWithAnswerCountDto> GetSurveyByIdAsync(Guid id);
    public Task<SurveyWithAnswerCountDto> UpdateSurveyAsync(Guid id, SurveyUpdateDto surveyUpdateDto);
}