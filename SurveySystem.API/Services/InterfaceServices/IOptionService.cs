using SurveySystem.API.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface IOptionService
{
    Task<IEnumerable<Option>> GetOptionsByQuestionIdAsync(Guid questionId);
    Task<Option?> GetOptionByIdAsync(Guid id);
    Task<Option> CreateOptionAsync(Option option);
    Task<Option> UpdateOptionAsync(Option option);
    Task<bool> DeleteOptionAsync(Guid id);
}