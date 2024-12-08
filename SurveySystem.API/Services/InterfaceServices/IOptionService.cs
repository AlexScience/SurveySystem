using SurveySystem.Models.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface IOptionService
{
    public Task<IEnumerable<Option>> GetOptionsByQuestionIdAsync(Guid questionId);
    public Task<Option?> GetOptionByIdAsync(Guid id);
    public Task<Option> CreateOptionAsync(Option option);
    public Task<bool> DeleteOptionAsync(Guid id);
}